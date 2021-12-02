using Domain.Entities.MeasurementBookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    class MeasurementBookConfiguration : IEntityTypeConfiguration<MeasurementBook>
    {
        public void Configure(EntityTypeBuilder<MeasurementBook> builder)
        {
            builder.Property(p => p.MeasurementOfficer).HasMaxLength(6).IsRequired();
            builder.Property(p => p.ValidatingOfficer).HasMaxLength(6).IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);

            // Backing fields
            builder.Navigation(p => p.Items).HasField("_items");
        }
    }
}
