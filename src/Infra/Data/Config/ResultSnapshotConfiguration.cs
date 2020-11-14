using AppCore.Domain.Entities.TestExecution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Config
{
    public class ResultSnapshotConfiguration : IEntityTypeConfiguration<ResultSnapshot>
    {
        public void Configure(EntityTypeBuilder<ResultSnapshot> builder)
        {
            builder.OwnsMany(
                o => o.ScenarioResultList,
                n =>
                {
                    n.WithOwner().HasForeignKey("OwnerId");
                    n.Property(n => n.ScenarioId)
                        .IsRequired();
                    n.Property(n => n.StatusReason)
                       .IsRequired();
                }
                );
        }
    }
}
