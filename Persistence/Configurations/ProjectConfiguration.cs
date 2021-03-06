using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
             builder.Property(p => p.Name).HasMaxLength(20).IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);
        }
    }
}