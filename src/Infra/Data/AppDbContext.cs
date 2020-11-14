using AppCore.Domain.Entities.TestRepository;
using AppCore.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Domain.Entities.TestExecution;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AppCore.Domain.Entities;

namespace Infra.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
            ChangeTracker.StateChanged += UpdateTimestamps;
            ChangeTracker.Tracked += UpdateTimestamps;
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TestSuite> TestSuites { get; set; }
        public DbSet<ResultSnapshot> ResultSnapshots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static void UpdateTimestamps(object sender, EntityEntryEventArgs e)
        {
            if (e.Entry.Entity is AuditableEntity entityWithTimestamps)
            {
                switch (e.Entry.State)
                {
                    case EntityState.Deleted:
                        entityWithTimestamps.LastModified = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entityWithTimestamps.LastModified = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        entityWithTimestamps.Created = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
