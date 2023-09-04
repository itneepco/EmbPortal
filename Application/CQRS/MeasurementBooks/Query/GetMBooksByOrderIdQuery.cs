using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Query
{
    public record GetMBooksByOrderIdQuery(int workOrderId) : IRequest<List<MBookResponse>>
    {
    }

    public class GetMBooksByOrderIdQueryHandler : IRequestHandler<GetMBooksByOrderIdQuery, List<MBookResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetMBooksByOrderIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<MBookResponse>> Handle(GetMBooksByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var userQuery = _context.AppUsers.AsQueryable();
            var orderQuery = _context.WorkOrders.Include(p => p.Items).AsQueryable();
            var mBooksQuery = _context.MeasurementBooks
                .Include(p => p.Items)
                .Where(p => p.WorkOrderId == request.workOrderId).AsQueryable();

            var query = from mBook in mBooksQuery
                        join wOrder in orderQuery on mBook.WorkOrderId equals wOrder.Id
                        join measurer in userQuery on mBook.MeasurerEmpCode equals measurer.UserName
                        join validator in userQuery on mBook.ValidatorEmpCode equals validator.UserName
                        join eic in userQuery on mBook.EicEmpCode equals eic.UserName
                        select new { mBook, wOrder, measurer, validator, eic };
            
            var results = await query.ToListAsync();
            List<MBookResponse> mBookResponses = new();

            foreach (var result in results)
            {
                var mBookResponse = _mapper.Map<MBookResponse>(result.mBook);

                mBookResponse.OrderNo = result.wOrder.OrderNo.ToString();
                mBookResponse.OrderDate = result.wOrder.OrderDate;
                mBookResponse.Contractor = result.wOrder.Contractor;
                mBookResponse.MeasurerName = result.measurer.DisplayName;
                mBookResponse.ValidatorName = result.validator.DisplayName;
                mBookResponse.EicEmpCode = result.eic.DisplayName;

                foreach (var item in mBookResponse.Items)
                {
                    var wOrderItem = result.wOrder.Items.FirstOrDefault(p => p.Id == item.WorkOrderItemId);
                    if (wOrderItem == null)
                    {
                        throw new NotFoundException($"Item does not exists with Id: {item.WorkOrderItemId}");
                    }

                    item.ItemNo = wOrderItem.ItemNo;
                    item.ItemDescription = wOrderItem.ItemDescription;
                    item.ServiceNo = wOrderItem.ServiceNo;
                    item.SubItemNo = wOrderItem.SubItemNo;
                    item.SubItemPackageNo = wOrderItem.SubItemPackageNo;
                    item.PackageNo = wOrderItem.PackageNo;
                    item.PoQuantity = wOrderItem.PoQuantity;
                    item.Uom = wOrderItem.Uom;
                    item.UnitRate = wOrderItem.UnitRate;
                    item.ShortServiceDesc = wOrderItem.ShortServiceDesc;
                }

                mBookResponses.Add(mBookResponse);
            }

            return mBookResponses;
        }
    }
}
