using Domian.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class WorkOrderItemConfiguration : IEntityTypeConfiguration<WorkOrderItem>
    {
        public void Configure(EntityTypeBuilder<WorkOrderItem> builder)
        {
             builder.Property(p => p.Name).IsRequired();
             builder.Property(p => p.No).IsRequired();
             builder.Property(p => p.Rate).IsRequired();
             builder.Property(p => p.UomId).IsRequired();
             builder.Property(p => p.PoQuantity).IsRequired();
        }
    }
}