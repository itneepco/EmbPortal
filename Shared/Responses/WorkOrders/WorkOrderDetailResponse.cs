using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class WorkOrderDetailResponse : WorkOrderResponse
    {
        public List<WorkOrderItemResponse> Items { get; set; }
    }
}
