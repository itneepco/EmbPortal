using Domain.Entities.RABillAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.RecurringAccountBill;

public class RABillItemConfiguration : IEntityTypeConfiguration<RABillItem>
{
    public void Configure(EntityTypeBuilder<RABillItem> builder)
    {
        builder.Property(p => p.Remarks).HasMaxLength(PersistenceConsts.RemarksLegth);

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);

        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);
    }
}
