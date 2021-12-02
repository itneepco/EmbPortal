namespace Shared.Requests
{
    public class WorkOrderItemRequest
    {
        public string Description { get; set; }
        public int ItemNo { get; set; }
        public int UomId { get; set; }
        public decimal UnitRate { get; set; }
        public float PoQuantity { get; set; }
    }
}
