using Domain.Common;

namespace Domain.Entities.MBSheetAggregate
{
    public class MBSheetItem :AuditableEntity
    {
        public int Id { get; private set; }
        public int Nos { get; private set; }
        public string Description { get; private set; }
        public string AttachmentUrl { get; private set; }
        public float Value1 { get; private set; }
        public float Value2 { get; private set; }
        public float Value3 { get; private set; }
        public string Uom { get; private set; }
        public decimal UnitRate { get; private set; }
        public int Dimension { get; private set; }
        
        public int MBookItemId { get; private set; }
        public string MBookItemDescription { get; private set; }
        
        public int MBSheetId { get; set; }
        public MBSheet MBSheet { get; set; }

        public MBSheetItem(string mbItemDescription, string description, string uom, int dimension, decimal rate, int mBookItemId, int nos, float value1, float value2, float value3)
        {
            MBookItemDescription = mbItemDescription;
            Description = description;
            Uom = uom;
            Dimension = dimension;
            UnitRate = rate;
            MBookItemId = mBookItemId;
            Nos = nos;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public MBSheetItem()
        {
        }

        public void SetDescription(string desc)
        {
            Description = desc;
        }

        public void SetNos(int no)
        {
            Nos = no;
        }

        public void SetValue1(float value1)
        {
            Value1 = value1;
        }

        public void SetValue2(float value2)
        {
            Value2 = value2;
        }

        public void SetValue3(float value3)
        {
            Value3 = value3;
        }
    }
}
