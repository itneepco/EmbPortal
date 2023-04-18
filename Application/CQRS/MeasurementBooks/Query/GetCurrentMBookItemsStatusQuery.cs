using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.MeasurementBookAggregate;
using Domain.Entities.WorkOrderAggregate;
using EmbPortal.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.MeasurementBooks.Query;

public record GetCurrentMBookItemsStatusQuery(int MBookId) : IRequest<List<MBItemStatusResponse>>
{
}

public class GetCurrentMBookItemsStatusQueryHandler : IRequestHandler<GetCurrentMBookItemsStatusQuery, List<MBItemStatusResponse>>
{
    private readonly IAppDbContext _context;
    private readonly IMeasurementBookService _mBookService;
    private readonly IRABillService _raBillService;

    public GetCurrentMBookItemsStatusQueryHandler(IAppDbContext context, IMeasurementBookService mBookService, IRABillService raBillService)
    {
        _context = context;
        _mBookService = mBookService;
        _raBillService = raBillService;
    }

    public async Task<List<MBItemStatusResponse>> Handle(GetCurrentMBookItemsStatusQuery request, CancellationToken cancellationToken)
    {
        var wOrderQuery =  _context.WorkOrders.Include(p => p.Items).AsQueryable();

        var mBookQuery =  _context.MeasurementBooks.Include(p => p.Items).AsQueryable();

        var query = from mBook in mBookQuery
                    join wOrder in wOrderQuery
                    on mBook.WorkOrderId equals wOrder.Id
                    select new { mBook, wOrder };

        var result = await query.FirstOrDefaultAsync(p => p.mBook.Id == request.MBookId);

        
        if (result == null)
        {
            throw new NotFoundException(nameof(MeasurementBook), request.MBookId);
        }



        // Fetch the MB items status
        List<MBookItemQtyStatus> mbItemQtyStatuses = await _mBookService.GetMBItemsQtyStatus(result.mBook.Id);

        // Fetch the cumulative RA items quantity
        List<RAItemQtyStatus> raItemQtyStatuses = await _raBillService.GetRAItemQtyStatus(result.mBook.Id);

        List<MBItemStatusResponse> itemStatusResponses = new();
        foreach (var item in result.mBook.Items)
        {
            var mbItemQtyStatus = mbItemQtyStatuses.Find(i => i.WorkOrderItemId == item.WorkOrderItemId);
            var raItemQtyStatus = raItemQtyStatuses.Find(i => i.WorkOrderItemId == item.WorkOrderItemId);
            var workOrderItem = result.wOrder.Items.FirstOrDefault(i => i.Id == item.WorkOrderItemId);
            if(workOrderItem == null) {
                throw new NotFoundException(nameof(WorkOrderItem), item.WorkOrderItemId);
            }

            itemStatusResponses.Add(new MBItemStatusResponse
            {
                MBookItemId = item.Id,
                WorkOrderItemId = workOrderItem.Id,
                ItemDescription = workOrderItem.ShortServiceDesc,
                UnitRate = workOrderItem.UnitRate,                    
                Uom = workOrderItem.Uom,
                PoQuantity = workOrderItem.PoQuantity,
                CumulativeMeasuredQty = mbItemQtyStatus != null ? mbItemQtyStatus.TotalMeasuredQty : 0,
                AcceptedMeasuredQty = mbItemQtyStatus != null ? mbItemQtyStatus.AcceptedMeasuredQty : 0,
                TillLastRAQty = raItemQtyStatus != null ? raItemQtyStatus.ApprovedRAQty : 0
            });
        }

        return itemStatusResponses;
    }
}
