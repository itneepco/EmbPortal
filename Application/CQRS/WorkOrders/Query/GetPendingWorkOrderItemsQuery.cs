using Application.Interfaces;
using EmbPortal.Shared.Enums;
using EmbPortal.Shared.Responses;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.WorkOrders.Query
{
    public record GetPendingWorkOrderItemsQuery(int workOrderId) : IRequest<IReadOnlyList<PendingOrderItemResponse>>
    {
    }

    public class GetPendingWorkOrderItemsQueryHandler : IRequestHandler<GetPendingWorkOrderItemsQuery, IReadOnlyList<PendingOrderItemResponse>>
    {
        private readonly IWorkOrderService _orderService;

        public GetPendingWorkOrderItemsQueryHandler(IWorkOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IReadOnlyList<PendingOrderItemResponse>> Handle(GetPendingWorkOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var workOrder = await _orderService.GetWorkOrderWithItems(request.workOrderId);

            List<PendingOrderItemResponse> response = new();

            foreach (var item in workOrder.Items)
            {
                // If work order item is already taken in some measurement book or is not yet published
                if (item.MBookItem != null || item.Status != WorkOrderItemStatus.PUBLISHED) continue;

                response.Add(new PendingOrderItemResponse
                {
                    ItemId = item.Id,
                    Description = item.Description
                });
            }

            return response.AsReadOnly();
        }
    }
}
