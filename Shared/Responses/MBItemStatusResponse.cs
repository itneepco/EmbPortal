namespace EmbPortal.Shared.Responses
{
    public class MBItemStatusResponse
    {
        public int MBookItemId { get; set; }
        public string ItemDescription { get; set; }
        public int Dimension { get; set; }
        public string Uom { get; set; }
        public decimal UnitRate { get; set; }
        public float PoQuantity { get; set; }
        public float CumulativeMeasuredQty { get; set; }
        public float AcceptedMeasuredQty { get; set; }
        public float TillLastRAQty { get; set; }

        public float AvailableRAQty
        {
            get
            {
                return AcceptedMeasuredQty - TillLastRAQty;
            }
        }

        public float AvailableMeasurementQty
        {
            get
            {
                return PoQuantity - CumulativeMeasuredQty;
            }
        }
    }
}
