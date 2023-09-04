namespace EmbPortal.Shared.Responses
{
    public class RABillItemResponse
    {
        public int Id { get; set; }
        public int WorkOrderItemId { get; set; }
        public string UoM { get; set; }
        public float PoQuantity { get; set; }
        public int ItemNo { get; set; }
        public string ItemDescription { get; set; }
        public int SubItemNo { get; set; }
        public long ServiceNo { get; set; }
        public string ShortServiceDesc { get; set; }
        public decimal UnitRate { get; set; }
        public float AcceptedMeasuredQty { get; set; }
        public float TillLastRAQty { get; set; }
        public float CurrentRAQty { get; set; }
        public string Remarks { get; set; }

        public decimal CurrentRAAmount => (decimal)CurrentRAQty * UnitRate;
    }
}
