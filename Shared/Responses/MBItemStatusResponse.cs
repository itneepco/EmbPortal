namespace EmbPortal.Shared.Responses
{
    public class MBItemStatusResponse
    {
        public int MBookItemId { get; set; }
        public string ItemDescription { get; set; }
        public decimal UnitRate { get; set; }
        public float AcceptedMeasuredQty { get; set; }
        public float TillLastRAQty { get; set; }

        public float AvailableQty
        {
            get
            {
                return AcceptedMeasuredQty - TillLastRAQty;
            }
        }
    }
}
