using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;

namespace Domain.Entities.WorkOrderAggregate
{
    public class SubItem :AuditableEntity
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public int UomId { get; private set; } 
        public decimal UnitRate { get; private set; }
        public float PoQuantity { get; private set; }
        public Uom Uom { get; private set; }

        private SubItem()
        {
        }

        public SubItem(string description, int uomId,decimal unitRate, float poQuantity)
        {
            Description = description;
            UomId = uomId;
            UnitRate = unitRate;            
            PoQuantity = poQuantity;
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
    }
}