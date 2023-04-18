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
                        join order in orderQuery on mBook.WorkOrderId equals order.Id
                        join measurer in userQuery on mBook.MeasurerEmpCode equals measurer.UserName
                        join validator in userQuery on mBook.ValidatorEmpCode equals validator.UserName
                        select new MBookResponse
                        {
                            Id = mBook.Id,
                            Title = mBook.Title,
                            ValidatorEmpCode = mBook.ValidatorEmpCode,
                            ValidatorName = validator.DisplayName,
                            MeasurerEmpCode = mBook.MeasurerEmpCode,
                            MeasurerName = measurer.DisplayName,
                            Status = mBook.Status.ToString(),
                            Items = order.Items.Select(p => new MBookItemResponse
                            { 
                                WorkOrderItemId = p.Id,
                                ServiceNo = p.ServiceNo,
                                ItemNo = p.ItemNo,
                                ItemDescription = p.ItemDescription,
                                PackageNo = p.PackageNo,
                                PoQuantity = p.PoQuantity,
                                SubItemNo = p.SubItemNo,
                                SubItemPackageNo = p.SubItemPackageNo,
                                ShortServiceDesc = p.ShortServiceDesc,
                                UnitRate = p.UnitRate,
                                Uom = p.Uom
                            }).ToList()
                        };
            
            var result = await query.ToListAsync();


            return result;
        }
    }
}
