using Domain.Entities.MBSheetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Persistence.Configurations
{
    public class ItemAttachmentConfiguration : IEntityTypeConfiguration<ItemAttachment>
    {
        public void Configure(EntityTypeBuilder<ItemAttachment> builder)
        {
            builder.Property(p => p.FileName)
                .HasMaxLength(PersistenceConsts.FileNameLegth)
                .IsRequired();

            builder.Property(p => p.FileNormalizedName)
                .HasMaxLength(PersistenceConsts.FileNameLegth)
                .IsRequired();

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);

            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);
        }
    }
}
