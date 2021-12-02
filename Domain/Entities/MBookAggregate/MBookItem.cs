using Domain.Common;
using Domain.Entities.WorkOrderAggregate;

namespace Domain.Entities.MeasurementBookAggregate
{
    public class MBookItem : AuditableEntity
    {
        public int Id { get; private set; }
        public int WorkOrderItemId { get; set; }
        public float Quantity { get; set; }

        // Navigation Property
        public WorkOrderItem WorkOrderItem { get; set; }

        public MBookItem()
        {
        }

        public MBookItem(int wOrderItemId, float quantity)
        {
            WorkOrderItemId = wOrderItemId;
            Quantity = quantity;
        }

        public void SetWorkOrderItemNo(int wOrderItemId)
        {
            WorkOrderItemId = wOrderItemId;
        }

        public void SetQuantity(float quantity)
        {
            Quantity = quantity;
        }
    }
}