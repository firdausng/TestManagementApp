using AppCore.Domain.Entities.TestExecution;
using AppCore.Domain.Entities.TestRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Common.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TestSuite> TestSuites { get; set; }
        public DbSet<ResultSnapshot> ResultSnapshots { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
