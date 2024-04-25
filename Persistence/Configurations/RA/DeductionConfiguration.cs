using Domain.Entities.RAAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.RA;

public class DeductionConfiguration : IEntityTypeConfiguration<Deduction>
{
    public void Configure(EntityTypeBuilder<Deduction> builder)
    {
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(PersistenceConsts.LongDescLength);

        builder.Property(p => p.Amount)
               .IsRequired()
               .HasPrecision(18, 2);
    }
}
