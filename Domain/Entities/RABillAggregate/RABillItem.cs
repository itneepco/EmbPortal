using Domain.Common;

namespace Domain.Entities.RABillAggregate
{
    public class RABillItem : AuditableEntity
    {
        public int Id { get; private set; }
        public string ItemDescription { get; private set; }
        public decimal UnitRate { get; private set; }
        public float AcceptedMeasuredQty { get; private set; }
        public float TillLastRAQty { get; private set; }
        public float CurrentRAQty { get; private set; }
        public string Remarks { get; private set; } // character 100 max limit
        public int MBookItemId { get; private set; }
        public int RABillId { get; set; }
        public RABill RABill { get; set; }

        public RABillItem()
        {
        }

        public RABillItem(int mbItemId, string desc, decimal rate, float acceptedMeasuredQty, float tillLastRAQty, float currentRAQty, string remarks)
        {
            MBookItemId = mbItemId;
            ItemDescription = desc;
            UnitRate = rate;
            AcceptedMeasuredQty = acceptedMeasuredQty;
            TillLastRAQty = tillLastRAQty;
            CurrentRAQty = currentRAQty;
            Remarks = remarks;
        }
    }
}
