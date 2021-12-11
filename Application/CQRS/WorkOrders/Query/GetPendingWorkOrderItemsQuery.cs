using Application.Interfaces;
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
                response.Add(new PendingOrderItemResponse
                {
                    ItemId = item.Id,
                    Description = item.Description,
                    IsPending = item.MBookItem == null ? true : false
                }); ;
            }

            return response.AsReadOnly();
        }
    }
}
