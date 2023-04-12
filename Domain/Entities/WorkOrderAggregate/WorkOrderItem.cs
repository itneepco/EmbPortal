using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Enums;

namespace Domain.Entities.WorkOrderAggregate
{
    public class WorkOrderItem : AuditableEntity
    {
        public int Id { get; private set; }
        public int ItemNo { get;  set; }
        public string PackageNo { get;  set; }
        public string ItemDescription { get; set; }
        public int SubItemNo { get;  set; }
        public string SubItemPackageNo { get;  set; }
        public long ServiceNo { get;  set; }
        public string ShortServiceDesc { get;  set; }
        public string LongServiceDesc { get;  set; }       
        public string Uom { get;  set; }
        public decimal UnitRate { get;  set; }
        public float PoQuantity { get;  set; }       
        
        public MBookItem MBookItem { get;  set; }

        public WorkOrderItem()
        {
        }

        public WorkOrderItem(
            int itemNo,
            string packageNo,
            string itemDesc,
            int subItemNo,
            string subItemPackageNo,
            long serviceNo,
            string shortServiceDesc,
            string longServiceDesc,
            string uom,
            decimal unitRate,
            float poQuantity
        )
        {
            ItemNo = itemNo;
            PackageNo = packageNo;
            ItemDescription = itemDesc;
            SubItemNo = subItemNo;
            SubItemPackageNo =subItemPackageNo;
            ServiceNo = serviceNo;
            ShortServiceDesc = shortServiceDesc;
            LongServiceDesc = longServiceDesc;
            Uom = uom;
            UnitRate = unitRate;
            PoQuantity = poQuantity;
          
        }
               
              

    }
}
