using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class WorkOrderDetailResponse : WorkOrderResponse
    {
        public IReadOnlyList<WorkOrderItemResponse> Items { get; set; }
    }
}
