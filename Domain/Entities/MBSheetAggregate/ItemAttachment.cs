using Domain.Common;

namespace Domain.Entities.MBSheetAggregate
{
    public class ItemAttachment : AuditableEntity
    {
        public int Id { get; private set; }
        public string FileName { get; private set; }
        public string FileNormalizedName { get; private set; }
        public int MBSheetItemId { get; private set; }
        public MBSheetItem MBSheetItem { get; private set; }

        public ItemAttachment(string fileName, string storedFileName)
        {
            FileName = fileName;
            FileNormalizedName = storedFileName;
        }

        public ItemAttachment()
        {
        }
    }
}
