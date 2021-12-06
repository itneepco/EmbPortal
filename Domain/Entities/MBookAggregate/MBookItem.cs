using Domain.Common;
using Domain.Entities.WorkOrderAggregate;

namespace Domain.Entities.MeasurementBookAggregate
{
    public class MBookItem : AuditableEntity
    {
        public int Id { get; private set; }
        public int WorkOrderItemId { get; set; }
        public int MeasurementBookId { get; set; }
        public SubItem WorkOrderItem { get; set; }

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