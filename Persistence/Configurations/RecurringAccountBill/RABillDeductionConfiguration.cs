using Domain.Entities.RABillAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.RecurringAccountBill;

public class RABillDeductionConfiguration : IEntityTypeConfiguration<RADeduction>
{
    public void Configure(EntityTypeBuilder<RADeduction> builder)
    {
        builder.Property(p => p.Amount)
                   .HasPrecision(18, 2);
    }
}
