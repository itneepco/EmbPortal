using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;
using EmbPortal.Shared.Enums;

namespace Domain.Entities.WorkOrderAggregate
{
    public class WorkOrderItem : AuditableEntity
    {
        public int Id { get; private set; }
        public string ServiceNo { get; private set; }
        public string Description { get; private set; }
        public int UomId { get; private set; }
        public decimal UnitRate { get; private set; }
        public float PoQuantity { get; private set; }
        public Uom Uom { get; private set; }
        public WorkOrderItemStatus Status { get; set; }
        public MBookItem MBookItem { get; private set; }
        public WorkOrderItem()
        {
        }

        public WorkOrderItem(string description, int uomId, decimal unitRate, float poQuantity)
        {
            Description = description;
            UomId = uomId;
            UnitRate = unitRate;
            PoQuantity = poQuantity;
            Status = WorkOrderItemStatus.CREATED;
        }
        public void SetDescription(string description)
        {
            Description = description;
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
