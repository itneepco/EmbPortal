using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Enums;

namespace Domain.Entities.WorkOrderAggregate
{
    public class WorkOrderItem : AuditableEntity
    {
        public int Id { get; private set; }
        public string ItemNo { get; private set; }
        public string ItemDescription { get; set; }
        public string SubItemNo { get; private set; }
        public string ServiceNo { get; private set; }
        public string ShortServiceDesc { get; private set; }
        public string LongServiceDesc { get; private set; }
        public int UomId { get; private set; }
        public decimal UnitRate { get; private set; }
        public float PoQuantity { get; private set; }
        public Uom Uom { get; private set; }
        public WorkOrderItemStatus Status { get; set; }
        public MBookItem MBookItem { get; private set; }

        public WorkOrderItem()
        {
        }

        public WorkOrderItem(
            string itemNo,
            string itemDesc,
            string subItemNo,
            string serviceNo,
            string shortServiceDesc,
            string longServiceDesc,
            int uomId, 
            decimal unitRate, 
            float poQuantity
        )
        {
            SetItemDescription(itemNo);
            SetItemDescription(itemDesc);
            SetItemDescription(subItemNo);
            SetItemDescription(serviceNo);
            SetShortServiceDesc(shortServiceDesc);
            SetLongServiceDesc(longServiceDesc);
            SetUomId(uomId);
            SetUnitRate(unitRate);
            SetPoQuantity(poQuantity);

            Status = WorkOrderItemStatus.CREATED;
        }

        public void SetItemNo(string itemNo)
        {
            ItemNo = itemNo;
        }

        public void SetSubItemNo(string subItemNo)
        {
            SubItemNo = subItemNo;
        }

        public void SetServiceNo(string serviceNo)
        {
            ServiceNo = serviceNo;
        }

        public void SetItemDescription(string description)
        {
            ItemDescription = description;
        }

        public void SetShortServiceDesc(string description)
        {
            ShortServiceDesc = description;
        }

        public void SetLongServiceDesc(string description)
        {
            LongServiceDesc = description;
        }

        public void SetUomId(int uomId)
        {
            UomId = uomId;
        }

        public void SetUnitRate(decimal unitRate)
        {
            UnitRate = unitRate;
        }

        public void SetPoQuantity(float poQuantity)
        {
            PoQuantity = poQuantity;
        }

        public void MarkPublished()
        {
            Status = WorkOrderItemStatus.PUBLISHED;
        }
    }
}
