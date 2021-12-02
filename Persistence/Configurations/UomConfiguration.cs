using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UomConfiguration : IEntityTypeConfiguration<Uom>
    {
        public void Configure(EntityTypeBuilder<Uom> builder)
        {
             builder.Property(p => p.Name).HasMaxLength(20).IsRequired();
             builder.Property(p => p.Dimension).IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);
        }
    }
}