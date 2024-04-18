using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Responses;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Query;

public record GetMBookByIdQuery(int Id) : IRequest<MBookResponse>
{
}

public class GetMBookByIdQueryHandler : IRequestHandler<GetMBookByIdQuery, MBookResponse>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMeasurementBookService _mBookService;

    private Expression<Func<MeasurementBook, bool>> Criteria { set; get; }

    public GetMBookByIdQueryHandler(IMapper mapper, IAppDbContext context, ICurrentUserService currentUserService, IMeasurementBookService mBookService)
    {
        _mapper = mapper;
        _context = context;
        _currentUserService = currentUserService;
        _mBookService = mBookService;
    }

    public async Task<MBookResponse> Handle(GetMBookByIdQuery request, CancellationToken cancellationToken)
    {
        var wOrderQuery = _context.WorkOrders.Include(p => p.Items).AsQueryable();
        var userQuery = _context.AppUsers.AsQueryable();        
        var mbBookQuery = _context.MeasurementBooks
                             .Include(p => p.Items)                             
                             .AsQueryable();
        
        var query = from mBook in mbBookQuery
                    join wOrder in wOrderQuery on mBook.WorkOrderId equals wOrder.Id
                    join measurer in userQuery on mBook.MeasurerEmpCode equals measurer.UserName
                    join validator in userQuery on mBook.ValidatorEmpCode equals validator.UserName
                    join eic in userQuery on mBook.EicEmpCode equals eic.UserName
                    select new
                    {
                        mBook, wOrder,measurer, validator, eic
                    };

        var result = await query
            .Where(m => m.mBook.MeasurerEmpCode == _currentUserService.EmployeeCode ||
                        m.mBook.ValidatorEmpCode == _currentUserService.EmployeeCode ||
                        m.mBook.EicEmpCode == _currentUserService.EmployeeCode)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.mBook.Id == request.Id);

        if (result == null)
        {
            throw new NotFoundException(nameof(MeasurementBook), request.Id);
        }

        var mBookResponse = _mapper.Map<MBookResponse>(result.mBook);

        mBookResponse.OrderNo = result.wOrder.OrderNo.ToString();
        mBookResponse.OrderDate = result.wOrder.OrderDate;
        mBookResponse.Contractor = result.wOrder.Contractor;
        mBookResponse.MeasurerName = result.measurer.DisplayName;
        mBookResponse.ValidatorName = result.validator.DisplayName;
        mBookResponse.EicEmpCode = result.eic.DisplayName;

        // Fetch the MB items status
        List<MBookItemQtyStatus> mbItemQtyStatuses = await _mBookService.GetMBItemsQtyStatus(result.mBook.Id);

        foreach (var item in mBookResponse.Items)
        {
            var wOrderItem = result.wOrder.Items.FirstOrDefault(p => p.Id == item.WorkOrderItemId);
            if(wOrderItem == null)
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
    

            var mbItemQtyStatus = mbItemQtyStatuses.Find(i => i.WorkOrderItemId == item.WorkOrderItemId);
            
            item.AcceptedMeasuredQty = mbItemQtyStatus != null ? mbItemQtyStatus.AcceptedMeasuredQty : 0;
            item.CumulativeMeasuredQty = mbItemQtyStatus != null ? mbItemQtyStatus.TotalMeasuredQty : 0;
        }
        // --- END ---

        return mBookResponse;
    }
}
