namespace EmbPortal.Shared.Responses
{
    public class RABillItemResponse
    {
        public int Id { get; set; }
        public int MBookItemId { get; set; }
        public string MBookItemDescription { get; set; }
        public decimal UnitRate { get; set; }
        public float AcceptedMeasuredQty { get; set; }
        public float TillLastRAQty { get; set; }
        public float CurrentRAQty { get; set; }
        public string Remarks { get; set; }

        public decimal CurrentRAAmount => (decimal)CurrentRAQty * UnitRate;
    }
}
