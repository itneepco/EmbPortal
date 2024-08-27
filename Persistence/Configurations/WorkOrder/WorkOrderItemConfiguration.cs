using Domain.Entities.WorkOrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class WorkOrderItemConfiguration : IEntityTypeConfiguration<WorkOrderItem>
{
    public void Configure(EntityTypeBuilder<WorkOrderItem> builder)
    {
        builder.Property(p => p.ItemNo).IsRequired();

        builder.Property(p => p.PackageNo)
            .IsRequired()
            .HasMaxLength(PersistenceConsts.PackageNoLength);
        
        builder.Property(p => p.ItemDescription)
            .HasMaxLength(PersistenceConsts.ShortDescLength)
            .IsRequired();

        builder.Property(p => p.SubItemNo).IsRequired();

        builder.Property(p => p.SubItemPackageNo)
            .IsRequired().HasMaxLength(PersistenceConsts.PackageNoLength);
        
        builder.Property(p => p.ServiceNo).IsRequired();

        builder.Property(p => p.ShortServiceDesc)
            .HasMaxLength(PersistenceConsts.ShortDescLength)
            .IsRequired();

        builder.Property(p => p.LongServiceDesc)
            .HasMaxLength(PersistenceConsts.LongDescLength);
        
        builder.Property(p => p.Uom).IsRequired();

        builder.Property(p => p.UnitRate)
               .IsRequired()
               .HasPrecision(18, 3);

        builder.Property(p => p.PoQuantity)
               .IsRequired()
               .HasPrecision(18, 3);

        builder.Property(p => p.MeasuredQuantity)
               .HasPrecision(18, 3);

        builder.Property(p => p.RAQuantity)
               .HasPrecision(18, 3);

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);

        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);
    }
}