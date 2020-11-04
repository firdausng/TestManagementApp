using AppCore.Domain.Entities.TestRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Config
{
    public class ScenarioConfiguration : IEntityTypeConfiguration<Scenario>
    {
        public void Configure(EntityTypeBuilder<Scenario> builder)
        {
            builder.OwnsMany(
                o => o.StepsList,
                n =>
                {
                    n.WithOwner().HasForeignKey("OwnerId");
                    n.Property(n => n.Order)
                        .IsRequired();
                    n.Property(n => n.Description)
                       .IsRequired();
                }
                );
        }
    }
}
