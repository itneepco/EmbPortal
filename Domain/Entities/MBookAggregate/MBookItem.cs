using Domain.Common;
using Domain.Entities.WorkOrderAggregate;

namespace Domain.Entities.MeasurementBookAggregate
{
    public class MBookItem : AuditableEntity
    {
        public int Id { get; private set; }
        public int WorkOrderItemId { get; private set; }
        public WorkOrderItem WorkOrderItem { get; private set; }

        public MBookItem()
        {
        }

        public MBookItem(int wOrderItemId)
        {
            WorkOrderItemId = wOrderItemId;
        }

        public void SetWorkOrderItemNo(int wOrderItemId)
        {
            WorkOrderItemId = wOrderItemId;
        }
    }
}