using Domian.WorkOrderAggregate;
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
        }
    }
}