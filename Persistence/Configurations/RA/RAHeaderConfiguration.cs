using Domain.Entities.RAAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.RA;

public class RAHeaderConfiguration : IEntityTypeConfiguration<RAHeader>
{
    public void Configure(EntityTypeBuilder<RAHeader> builder)
    {
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(PersistenceConsts.TitleLegth);

        builder.Property(p => p.Remarks)
            .IsRequired()
            .HasMaxLength(PersistenceConsts.LongDescLength);

        builder.Property(p => p.EicEmpCode)
            .IsRequired()
            .HasMaxLength(PersistenceConsts.EmpCodeLength);

        builder.Property(p => p.LastBillDetail)
            .IsRequired()
            .HasMaxLength(PersistenceConsts.LongDescLength);
    }
}
