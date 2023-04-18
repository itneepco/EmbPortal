using Domain.Entities.MBSheetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class MBSheetConfiguration : IEntityTypeConfiguration<MBSheet>
{
    public void Configure(EntityTypeBuilder<MBSheet> builder)
    {
        builder.Property(p => p.Title)
            .HasMaxLength(PersistenceConsts.TitleLegth)
            .IsRequired();

        builder.Property(p => p.MeasurerEmpCode)
            .HasMaxLength(PersistenceConsts.EmpCodeLength)
            .IsRequired();

        builder.Property(p => p.ValidatorEmpCode)
            .HasMaxLength(PersistenceConsts.EmpCodeLength)
            .IsRequired();

        builder.Property(p => p.EicEmpCode)
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
