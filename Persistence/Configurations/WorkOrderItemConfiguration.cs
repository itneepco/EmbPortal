using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class WorkOrderItemConfiguration : IEntityTypeConfiguration<WorkOrderItem>
    {
        public void Configure(EntityTypeBuilder<WorkOrderItem> builder)
        {
             builder.Property(p => p.Description).IsRequired();
             builder.Property(p => p.ItemNo).IsRequired();
             builder.Property(p => p.UnitRate).IsRequired();
             builder.Property(p => p.UomId).IsRequired();
             builder.Property(p => p.PoQuantity).IsRequired();
        }
    }
}