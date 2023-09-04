using Domain.Common;

namespace Domain.Entities.MeasurementBookAggregate;
public class MBookItem : AuditableEntity
{
    public int Id { get; private set; }
    public int WorkOrderItemId { get; set; }   

    public MBookItem()
    {
    }

    public MBookItem(int workOrderItemId)
    {
       
        WorkOrderItemId = workOrderItemId;
      
    }
}