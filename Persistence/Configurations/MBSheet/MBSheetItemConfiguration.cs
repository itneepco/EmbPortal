using Domain.Entities.MBSheetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class MBSheetItemConfiguration : IEntityTypeConfiguration<MBSheetItem>
    {
        public void Configure(EntityTypeBuilder<MBSheetItem> builder)
        {
            builder.Property(p => p.Uom)
                .HasMaxLength(PersistenceConsts.UomLength)
                .IsRequired();
            
            builder.Property(p => p.Description)
                .HasMaxLength(PersistenceConsts.LongDescLength)
                .IsRequired();

            builder.Property(p => p.ServiceNo).IsRequired();

            builder.Property(p => p.ShortServiceDesc)
                .HasMaxLength(PersistenceConsts.ShortDescLength)
                .IsRequired();

            // Backing fields
            builder.Navigation(p => p.Attachments).HasField("_attachments");

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);

            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);
        }
    }
}
