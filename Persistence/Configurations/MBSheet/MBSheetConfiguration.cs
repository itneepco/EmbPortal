using Microsoft.EntityFrameworkCore;
using Domain.Entities.MBSheetAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Identity;

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

            builder.HasOne(p => p.Measurer).WithOne()
                .HasPrincipalKey<AppUser>(p => p.UserName)
                .HasForeignKey<MBSheet>(p => p.MeasurementOfficer)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Validator).WithOne()
                .HasPrincipalKey<AppUser>(p => p.UserName)
                .HasForeignKey<MBSheet>(p => p.ValidationOfficer)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Acceptor).WithOne()
                .HasPrincipalKey<AppUser>(p => p.UserName)
                .HasForeignKey<MBSheet>(p => p.AcceptingOfficer)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);

            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);
        }
    }
}
