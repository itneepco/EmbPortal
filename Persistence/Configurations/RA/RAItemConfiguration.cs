using Domain.Entities.RAAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.RA;

public class RAItemConfiguration : IEntityTypeConfiguration<RAItem>
{
    public void Configure(EntityTypeBuilder<RAItem> builder)
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
            .IsRequired()
            .HasMaxLength(PersistenceConsts.PackageNoLength);

        builder.Property(p => p.ServiceNo).IsRequired();

        builder.Property(p => p.ShortServiceDesc)
            .HasMaxLength(PersistenceConsts.ShortDescLength)
            .IsRequired();

        builder.Property(p => p.Uom).IsRequired();

        builder.Property(p => p.UnitRate)
               .HasPrecision(18, 3);

        builder.Property(p => p.PoQuantity)
               .HasPrecision(18, 3);

        builder.Property(p => p.MeasuredQty)
               .HasPrecision(18, 3);

        builder.Property(p => p.TillLastRAQty)
               .HasPrecision(18, 3);

        builder.Property(p => p.CurrentRAQty)
               .HasPrecision(18, 3);

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);

        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);
    }
}
