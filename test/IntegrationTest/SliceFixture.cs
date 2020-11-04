using FluentValidation;
using Infra.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Respawn;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi;

namespace IntegrationTest
{
    public class SliceFixture
    {
        private static readonly Checkpoint _checkpoint;
        private static readonly IConfigurationRoot _configuration;
        private static readonly IServiceScopeFactory _scopeFactory;

        static SliceFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();

            var startup = new Startup(_configuration);
            var services = new ServiceCollection();
            services.AddLogging();
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            _scopeFactory = provider.GetService<IServiceScopeFactory>();
            _checkpoint = new Checkpoint
            {
                SchemasToInclude = new[]
                    {
                        "public"
                    },
                DbAdapter = DbAdapter.Postgres,
                //TablesToIgnore = new[]{}
            };
        }

        public static async Task ResetCheckpoint()
        {
            var connectionString = _configuration.GetConnectionString("PostgresConnectionString");
            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            await _checkpoint.Reset(conn);
        }

        public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

            try
            {
                //await dbContext.BeginTransactionAsync().ConfigureAwait(false);

                await action(scope.ServiceProvider).ConfigureAwait(false);

                //await dbContext.CommitTransactionAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                //dbContext.RollbackTransaction();
                throw;
            }
        }
        public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

            try
            {
                //await dbContext.BeginTransactionAsync().ConfigureAwait(false);

                var result = await action(scope.ServiceProvider).ConfigureAwait(false);

                //await dbContext.CommitTransactionAsync().ConfigureAwait(false);

                return result;
            }
            catch (Exception)
            {
                //dbContext.RollbackTransaction();
                throw;
            }
        }

        public static Task ExecuteDbContextAsync(Func<AppDbContext, IMediator, Task> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>(), sp.GetService<IMediator>()));

        public static Task<T> ExecuteDbContextAsync<T>(Func<AppDbContext, Task<T>> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>()));


        public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static Task<TResponse> SendWithValidationAsync<TResponse>(IRequest<TResponse> request, IValidator validator)
        {
            //var validationResult = validator.Validate(request);
            //var errorArray = validationResult.Errors.Select(err => err.ErrorMessage).ToArray();
            //var message = string.Join(",", errorArray);
            //validationResult.IsValid.ShouldBeTrue(message);

            return SendAsync(request);
        }

        public static Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static void WriteLog(string logMessage)
        {
            using var scope = _scopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetService<ILogger>();
            logger.LogInformation(logMessage);
        }
    }
}
