using Domian.Common;

namespace Domian.WorkOrderAggregate
{
    public class WorkOrderItem :AuditableEntity
    {
        private WorkOrderItem()
        {
        }

        public WorkOrderItem(string name, int no, int uomId, int poQuantity)
        {
            
            Name = name;
            No = no;
            UomId = uomId;            
            PoQuantity = poQuantity;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int No { get; private set; }   
        public int UomId { get; private set; } 
        public Uom Uom { get; private set; }
        public int PoQuantity { get; private set; }

        
    }
}