namespace EmbPortal.Shared.Responses
{
    public class MBookItemResponse
    {
        public int Id { get; set; }
        public int WorkOrderItemId { get; set; }
        public string Description { get; set; }
        public string Uom { get; set; }
        public decimal UnitRate { get; set; }
        public float PoQuantity { get; set; }

        public decimal TotalAmount
        {
            get
            {
                return (decimal)PoQuantity * UnitRate;
            }
        }
    }
}
