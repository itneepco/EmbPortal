using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Responses.WorkOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Application.CQRS.WorkOrders.Query;

public record WorkOrderItemStatusQuery(int workOrderId) : IRequest<List<WOItemStatusResponse>>
{
}

public class WorkOrderItemStatusQueryHandler : IRequestHandler<WorkOrderItemStatusQuery, List<WOItemStatusResponse>>
{
    private readonly IAppDbContext _context;
    public WorkOrderItemStatusQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WOItemStatusResponse>> Handle(WorkOrderItemStatusQuery request, CancellationToken cancellationToken)
    {
        var workorder = await _context.WorkOrders.Include(p => p.Items).FirstOrDefaultAsync(p => p.Id ==request.workOrderId);
        if (workorder == null)
        {
            throw new NotFoundException(nameof(MeasurementBook), request.workOrderId);
        }
        List<WOItemStatusResponse> itemStatusResponses = new();
        foreach (var item in workorder.Items)
        {
            if ((item.MeasuredQuantity - item.RAQuantity) > 0)
            {
                itemStatusResponses.Add(new WOItemStatusResponse
                {
                    WorkOrderItemId = item.Id,
                    ItemNo = item.ItemNo,
                    PackageNo = item.PackageNo,
                    ItemDescription = item.ItemDescription,
                    SubItemNo = item.SubItemNo,
                    ServiceNo = item.ServiceNo,
                    SubItemPackageNo = item.SubItemPackageNo,
                    ShortServiceDesc = item.ShortServiceDesc,
                    UnitRate = item.UnitRate,
                    Uom = item.Uom,
                    PoQuantity = item.PoQuantity,
                    MeasuredQty = item.MeasuredQuantity,
                    TillLastRAQty = item.RAQuantity
                });
            }
            
        }
        return itemStatusResponses;
    }
}
