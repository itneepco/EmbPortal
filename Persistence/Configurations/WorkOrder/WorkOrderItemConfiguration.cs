using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class WorkOrderItemConfiguration : IEntityTypeConfiguration<WorkOrderItem>
    {
        public void Configure(EntityTypeBuilder<WorkOrderItem> builder)
        {
            builder.Property(p => p.ItemNo).HasMaxLength(10).IsRequired();
            builder.Property(p => p.ItemDescription).HasMaxLength(50).IsRequired();
            builder.Property(p => p.SubItemNo).HasMaxLength(10).IsRequired();
            builder.Property(p => p.ServiceNo).HasMaxLength(10).IsRequired();
            builder.Property(p => p.ShortServiceDesc).HasMaxLength(50).IsRequired();
            builder.Property(p => p.LongServiceDesc).HasMaxLength(250).IsRequired();
            builder.Property(p => p.ServiceNo).HasMaxLength(10);
            builder.Property(p => p.UnitRate).IsRequired();
            builder.Property(p => p.UomId).IsRequired();
            builder.Property(p => p.PoQuantity).IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);

            builder.HasOne(p => p.Uom)
                .WithMany()
                .HasForeignKey(p => p.UomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}