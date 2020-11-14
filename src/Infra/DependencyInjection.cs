using AppCore.Common.Interfaces;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationsAssembly = typeof(DependencyInjection).Assembly.GetName().Name;

            services.AddDbContext<AppDbContext>((sp, cfg) =>
            {
                cfg.LogTo(Console.WriteLine,
                    new[]
                    {
                        DbLoggerCategory.Database.Command.Name
                    },
                    LogLevel.Information,
                    DbContextLoggerOptions.SingleLine | DbContextLoggerOptions.UtcTime);

                cfg
                    .UseNpgsql(configuration.GetConnectionString("PostgresConnectionString"),
                    options =>
                    {
                        options.EnableRetryOnFailure(3);
                        options.MigrationsAssembly(migrationsAssembly);
                    });
            });

            services.AddScoped<IAppDbContext, AppDbContext>();

            return services;
        }
    }
}
