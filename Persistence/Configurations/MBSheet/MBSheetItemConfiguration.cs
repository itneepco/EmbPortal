using Domain.Entities.MBSheetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class MBSheetItemConfiguration : IEntityTypeConfiguration<MBSheetItem>
    {
        public void Configure(EntityTypeBuilder<MBSheetItem> builder)
        {
            builder.Property(p => p.Uom).HasMaxLength(20).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(100).IsRequired();
            builder.Property(p => p.MBookItemDescription).HasMaxLength(250).IsRequired();
            builder.Property(p => p.AttachmentUrl).HasMaxLength(100);

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);
        }
    }
}
