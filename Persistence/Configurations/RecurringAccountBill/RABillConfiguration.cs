using Microsoft.EntityFrameworkCore;
using Domain.Entities.RABillAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Identity;

namespace Persistence.Configurations.RecurringAccountBill
{
    public class RABillConfiguration : IEntityTypeConfiguration<RABill>
    {
        public void Configure(EntityTypeBuilder<RABill> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(PersistenceConsts.TitleLegth)
                .IsRequired();
            builder.Property(p => p.AcceptingOfficer)
                .HasMaxLength(PersistenceConsts.EmpCodeLength).IsRequired();

            // Backing fields
            builder.Navigation(p => p.Items).HasField("_items");

            builder.HasOne(p => p.Acceptor).WithOne()
                .HasPrincipalKey<AppUser>(p => p.UserName)
                .HasForeignKey<RABill>(p => p.AcceptingOfficer)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);

            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);
        }
    }
}
