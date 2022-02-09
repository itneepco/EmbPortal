using Domain.Common;
using Domain.Entities.MeasurementBookAggregate;

namespace Domain.Entities.MBSheetAggregate
{
    public class MBSheetItem :AuditableEntity
    {
        public int Id { get; private set; }
        public int Nos { get; private set; }
        public string AttachmentUrl { get; private set; }
        public float Value1 { get; private set; }
        public float Value2 { get; private set; }
        public float Value3 { get; private set; }
        public string Description { get; private set; }
        public string Uom { get; private set; }
        public decimal UnitRate { get; private set; }
        public int Dimension { get; private set; }

        public int MBSheetId { get; set; }
        public MBSheet MBSheet { get; set; }

        public int MBookItemId { get; private set; }
        public MBookItem MBookItem { get; private set; }

        public MBSheetItem(string description, string uom, int dimension, decimal rate, int mBookItemId, float value1, float value2, float value3)
        {
            Description = description;
            Uom = uom;
            Dimension = dimension;
            UnitRate = rate;
            MBookItemId = mBookItemId;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public MBSheetItem()
        {
        }
    }
}
