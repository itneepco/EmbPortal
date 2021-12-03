namespace Shared.Responses
{
    public class WorkOrderItemResponse
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public string Description { get; set; }
        public int ItemNo { get; set; }
        public decimal UnitRate { get; set; }
        public UomResponse Uom { get; set; }
        public float PoQuantity { get; set; }
    }
}
