using Domain.Entities.Identity;
using Domain.Entities.WorkOrderAggregate;
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

            builder.Property(p => p.EngineerInCharge)
                .HasMaxLength(PersistenceConsts.EmpCodeLength)
                .IsRequired();

            builder.HasIndex(p => p.OrderNo).IsUnique();

            builder.HasMany(p => p.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

            // Backing fields
            builder.Navigation(p => p.Items).HasField("_items");
            builder.Navigation(p => p.MeasurementBooks).HasField("_measurementBooks");

            builder.HasOne(p => p.Engineer).WithOne()
                .HasPrincipalKey<AppUser>(p => p.UserName)
                .HasForeignKey<WorkOrder>(p => p.EngineerInCharge)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);

            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(PersistenceConsts.EmpCodeLength);
        }
    }
}