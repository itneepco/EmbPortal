using Domain.Entities.MeasurementBookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class MBookItemConfiguration : IEntityTypeConfiguration<MBookItem>
{
    public void Configure(EntityTypeBuilder<MBookItem> builder)
    {
        builder.Property(p => p.CreatedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);

        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);
    }
}
