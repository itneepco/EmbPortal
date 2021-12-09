namespace EmbPortal.Shared.Responses
{
    public class PendingOrderItemResponse
    {
        public int ItemId { get; set; }
        public string Description { get; set; }
        public bool IsPending { get; set; }
    }
}
