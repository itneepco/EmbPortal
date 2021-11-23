using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
    {
        public void Configure(EntityTypeBuilder<WorkOrder> builder)
        {
             builder.Property(p => p.OrderNo).IsRequired();
             builder.Property(p => p.OrderDate).IsRequired();
             builder.Property(p => p.Title).IsRequired();

            // Backing fields
            builder.Navigation(p => p.Items).HasField("_items");
            builder.Navigation(p => p.MeasurementBooks).HasField("_measurementBooks");
        }
    }
}