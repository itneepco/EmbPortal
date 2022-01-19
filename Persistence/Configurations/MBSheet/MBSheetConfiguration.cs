using Microsoft.EntityFrameworkCore;
using Domain.Entities.MBSheetAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class MBSheetConfiguration : IEntityTypeConfiguration<MBSheet>
    {
        public void Configure(EntityTypeBuilder<MBSheet> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();
            builder.Property(p => p.MeasurementOfficer).HasMaxLength(6).IsRequired();
            builder.Property(p => p.ValidationOfficer).HasMaxLength(6).IsRequired();
            builder.Property(p => p.AcceptingOfficer).HasMaxLength(6).IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);

            builder.HasMany(p => p.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

            // Backing fields
            builder.Navigation(p => p.Items).HasField("_items");
        }
    }
}
