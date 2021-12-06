using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public class WorkOrderItemResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<SubItemResponse> SubItems { get; set; }
    }
}
