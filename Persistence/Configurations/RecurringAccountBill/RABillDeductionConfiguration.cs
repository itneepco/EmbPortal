using Domain.Entities.RABillAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.RecurringAccountBill;

public class RABillDeductionConfiguration
{
    public void Configure(EntityTypeBuilder<RADeduction> builder)
    {
        builder.Property(p => p.Amount)
                   .HasPrecision(18, 2);
    }
}
