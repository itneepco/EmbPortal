using Domian.Common;

namespace Domian.WorkOrderAggregate
{
    public class WorkOrderItem :AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int No { get; set; }   
        public int UomId { get; set; } 
        public Uom Uom { get; set; }
        public int PoQuantity { get; set; }
    }
}