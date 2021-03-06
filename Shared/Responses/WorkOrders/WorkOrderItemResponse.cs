using EmbPortal.Shared.Enums;

namespace EmbPortal.Shared.Responses
{
    public class WorkOrderItemResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal UnitRate { get; set; }
        public string Uom { get; set; }
        public int UomId { get; set; }
        public int Dimension { get; set; }
        public float PoQuantity { get; set; }
        public WorkOrderItemStatus Status { get; set; }
    }
}
