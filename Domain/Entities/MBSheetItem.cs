using Domain.Common;

namespace Domain.Entities
{
    public class MBSheetItem :AuditableEntity
    {
        public int Id { get; set; }
        public int MBSheetId { get; set; }
        public int MBookItemId { get; set; }
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public float Value3 { get; set; }

    }
}
