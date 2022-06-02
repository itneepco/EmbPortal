using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class WorkOrderDetailResponse : WorkOrderResponse
    {
        public UserResponse Engineer { get; set; }
        public List<WorkOrderItemResponse> Items { get; set; }
    }
}
