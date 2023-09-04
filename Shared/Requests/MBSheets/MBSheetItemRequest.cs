using System.Collections.Generic;

namespace EmbPortal.Shared.Requests
{
    public class MBSheetItemRequest
    {
        public int Id { get; set; }
        public int WorkOrderItemId { get; set; }
        
        public List<MBItemMeasurementRequest> Measurements { get; set; } = new();
    }
}
