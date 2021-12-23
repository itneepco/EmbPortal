using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;

namespace Domain.Entities.MBSheetAggregate
{
    public class MBSheetItem :AuditableEntity
    {
        public int Id { get; private set; }
        public float Value1 { get; private set; }
        public float Value2 { get; private set; }
        public float Value3 { get; private set; }
        public string Description { get; private set; }
        public string Uom { get; private set; }
        public int Dimension { get; private set; }
        public int MBookItemId { get; private set; }
        public MBookItem MBookItem { get; private set; }
    }
}
