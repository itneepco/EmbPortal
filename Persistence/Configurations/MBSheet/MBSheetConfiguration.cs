using Microsoft.EntityFrameworkCore;
using Domain.Entities.MBSheetAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class MBSheetConfiguration : IEntityTypeConfiguration<MBSheet>
    {
        public void Configure(EntityTypeBuilder<MBSheet> builder)
        {
            builder.Property(p => p.Title)
                .HasMaxLength(PersistenceConsts.TitleLegth)
                .IsRequired();

            builder.Property(p => p.MeasurementOfficer)
                .HasMaxLength(PersistenceConsts.EmpCodeLength)
                .IsRequired();

            builder.Property(p => p.ValidationOfficer)
                .HasMaxLength(PersistenceConsts.EmpCodeLength)
                .IsRequired();

            builder.Property(p => p.AcceptingOfficer)
                .HasMaxLength(PersistenceConsts.EmpCodeLength)
                .IsRequired();

            // Backing fields
            builder.Navigation(p => p.Items).HasField("_items");

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);

            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);
        }
    }
}
