using Domain.Entities.MeasurementBookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
class MBookConfiguration : IEntityTypeConfiguration<MeasurementBook>
{
    public void Configure(EntityTypeBuilder<MeasurementBook> builder)
    {
        builder.Property(p => p.Title)
            .HasMaxLength(PersistenceConsts.TitleLegth)
            .IsRequired();

        builder.Property(p => p.MeasurementOfficer)
            .HasMaxLength(PersistenceConsts.EmpCodeLength)
            .IsRequired();

        builder.Property(p => p.ValidatingOfficer)
            .HasMaxLength(PersistenceConsts.EmpCodeLength)
            .IsRequired();

        builder.HasMany(p => p.Items)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Measurer).WithMany()
            .HasPrincipalKey(p => p.UserName)
            .HasForeignKey(p => p.MeasurementOfficer)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Validator).WithMany()
            .HasPrincipalKey(p => p.UserName)
            .HasForeignKey(p => p.ValidatingOfficer)
            .OnDelete(DeleteBehavior.Restrict);

        // Backing fields
        builder.Navigation(p => p.Items).HasField("_items");

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);

        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);
    }
}
