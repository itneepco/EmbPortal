using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
    {
        public void Configure(EntityTypeBuilder<WorkOrder> builder)
        {
            builder.Property(p => p.OrderNo).HasMaxLength(60).IsRequired();
            builder.Property(p => p.OrderDate).IsRequired();
            builder.Property(p => p.Title).HasMaxLength(250).IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);

            builder.HasMany(p => p.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

            // Backing fields
            builder.Navigation(p => p.Items).HasField("_items");
            builder.Navigation(p => p.MeasurementBooks).HasField("_measurementBooks");
        }
    }
}