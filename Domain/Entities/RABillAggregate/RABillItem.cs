using Domain.Common;

namespace Domain.Entities.RABillAggregate
{
    public class RABillItem : AuditableEntity
    {
        public int Id { get; private set; }
        public int ItemNo { get; private set; }
        public string PackageNo { get; private set; }
        public string ItemDescription { get; private set; }
        public int SubItemNo { get; private set; }
        public string SubItemPackageNo { get; private set; }
        public long ServiceNo { get; private set; }
        public string ShortServiceDesc { get; private set; }
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

        public RABillItem(
            int itemNo,
            string packageNo,
            string itemDescription,
            int subItemNo,
            string subItemPackageNo,
            long serviceNo,
            string serviceDescription,
            decimal rate,
            float acceptedMeasuredQty,
            float tillLastRAQty,
            float currentRAQty,
            string remarks,
            int mbItemId)
        {
            ItemNo = itemNo;
            PackageNo = packageNo;
            ItemDescription = itemDescription;
            SubItemNo = subItemNo;
            SubItemPackageNo = subItemPackageNo;
            ServiceNo = serviceNo;
            ShortServiceDesc = serviceDescription;
            UnitRate = rate;
            AcceptedMeasuredQty = acceptedMeasuredQty;
            TillLastRAQty = tillLastRAQty;
            CurrentRAQty = currentRAQty;
            Remarks = remarks;
            MBookItemId = mbItemId;
        }

        public void SetAcceptedMeasuredQty(float val)
        {
            AcceptedMeasuredQty = val;
        }
        public void SetTillLastRAQty(float val)
        {
            TillLastRAQty = val;
        }
        public void SetCurrentRAQty(float val)
        {
            CurrentRAQty = val;
        }
        public void SetRemarks(string val)
        {
            Remarks = val;
        }
    }
}
