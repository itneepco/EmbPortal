using Domain.Common;

namespace Domain.Entities.WorkOrderAggregate
{
    public class WorkOrderItem :AuditableEntity
    {
        private WorkOrderItem()
        {
        }

        public WorkOrderItem(string description, int itemNo, int uomId,decimal unitRate, float poQuantity)
        {
            
            Description = description;
            ItemNo = itemNo;
            UomId = uomId;
            UnitRate = unitRate;            
            PoQuantity = poQuantity;
        }

        public int Id { get; private set; }
        public string Description { get; private set; }
        public int ItemNo { get; private set; }   
        public int UomId { get; private set; } 
        public decimal UnitRate { get; set; }
        public Uom Uom { get; private set; }
        public float PoQuantity { get; private set; }
    }
}