using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    class SubItemConfiguration : IEntityTypeConfiguration<SubItem>
    {
        public void Configure(EntityTypeBuilder<SubItem> builder)
        {
            builder.Property(p => p.Description).HasMaxLength(100).IsRequired();
            builder.Property(p => p.UnitRate).IsRequired();
            builder.Property(p => p.UomId).IsRequired();
            builder.Property(p => p.PoQuantity).IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);
        }
    }
}
