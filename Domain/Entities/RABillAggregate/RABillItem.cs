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

        public RABillItem()
        {
        }
    }
}
