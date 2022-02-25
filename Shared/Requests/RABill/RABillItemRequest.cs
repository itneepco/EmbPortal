using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests
{
    public class RABillItemRequest
    {
        public int MBookItemId { get; set; }
        public string ItemDescription { get; set; }
        public decimal UnitRate { get; set; }
        public float AcceptedMeasuredQty { get; set; }
        public float TillLastRAQty { get; set; }
        public float CurrentRAQty { get; set; }

        [MaxLength(100)]
        public string Remarks { get; set; }

        public float AvailableQty => AcceptedMeasuredQty - TillLastRAQty;

        public decimal AvailableAmount => UnitRate * (decimal)AvailableQty;
    }
}
