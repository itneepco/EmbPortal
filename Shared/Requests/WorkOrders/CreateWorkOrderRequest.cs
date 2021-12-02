using System.Collections.Generic;

namespace Shared.Requests
{
    public class CreateWorkOrderRequest : WorkOrderRequest
    {
        public List<WorkOrderItemRequest> Items { get; set; } = new List<WorkOrderItemRequest>();
    }
}