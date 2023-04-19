using Domain.Entities.MBSheetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Persistence.Configurations;

public class MBItemMeasurementConfiguration : IEntityTypeConfiguration<MBItemMeasurement>
{
    public void Configure(EntityTypeBuilder<MBItemMeasurement> builder)
    {
        builder.Property(p => p.Description)
           .HasMaxLength(PersistenceConsts.LongDescLength);

        builder.Property(p => p.Val1)
           .HasMaxLength(PersistenceConsts.MeasuredQtyLegth);

        builder.Property(p => p.Val2)
           .HasMaxLength(PersistenceConsts.MeasuredQtyLegth);

        builder.Property(p => p.Val3)
           .HasMaxLength(PersistenceConsts.MeasuredQtyLegth);

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);

        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(PersistenceConsts.EmpCodeLength);
    }
}
