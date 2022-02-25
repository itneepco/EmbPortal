using Microsoft.EntityFrameworkCore;
using System;
using Domain.Entities.RABillAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.RecurringAccountBill
{
    public class RABillConfiguration : IEntityTypeConfiguration<RABill>
    {
        public void Configure(EntityTypeBuilder<RABill> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(6);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(6);

            // Backing fields
            builder.Navigation(p => p.Items).HasField("_items");
        }
    }
}
