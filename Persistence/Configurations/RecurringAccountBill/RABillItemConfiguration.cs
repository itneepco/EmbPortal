using Domain.Entities.RABillAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.RecurringAccountBill
{
    public class RABillItemConfiguration : IEntityTypeConfiguration<RABillItem>
    {
        public void Configure(EntityTypeBuilder<RABillItem> builder)
        {
            builder.Property(p => p.SubItemNo).HasMaxLength(10).IsRequired();
            builder.Property(p => p.ServiceNo).HasMaxLength(10).IsRequired();
            builder.Property(p => p.ItemDescription).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Remarks).HasMaxLength(100);

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);
        }
    }
}
