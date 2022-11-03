namespace EmbPortal.Shared.Responses
{
    public class PendingOrderItemResponse
    {
        public int WorkOrderItemId { get; set; }
        public int ItemNo { get; set; }
        public string ItemDescription { get; set; }
        public int SubItemNo { get; set; }
        public long ServiceNo { get; set; }
        public string ShortServiceDesc { get; set; }
    }
}
