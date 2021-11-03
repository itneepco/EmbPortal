using System.Collections.Generic;
using Domain.Common;
using Domain.Entities.WorkOrderAggregate;

namespace Domain.Entities.MeasurementBookAggregate
{
    public class MeasurementBook : AuditableEntity, IAggregateRoot
   {
      public int Id { get; set; }
      public int WorkOrderId { get; set; }
      public WorkOrder WorkOrder { get; set; }
      public string MeasurementOfficer { get; set; }
      public string ValidatingOfficer { get; set; }
      public bool Status { get; set; }
      private readonly List<MBItem> _lineItems = new List<MBItem>();
      public IReadOnlyList<MBItem> LineItems => _lineItems.AsReadOnly();
   }
}