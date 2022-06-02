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
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();
            builder.Property(p => p.AcceptingOfficer).HasMaxLength(6).IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);

            builder.HasOne(p => p.Acceptor).WithOne()
                .HasPrincipalKey<AppUser>(p => p.UserName)
                .HasForeignKey<RABill>(p => p.AcceptingOfficer);

            // Backing fields
            builder.Navigation(p => p.Items).HasField("_items");
        }
    }
}
