using System.Collections.Generic;
using Domian.Common;
using Domian.WorkOrderAggregate;

namespace Domian.MeasurementBookAggregate
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