using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GtMotive.Fleet.Api;
using GtMotive.Fleet.Infrastructure;
using GtMotive.Fleet.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit;

[assembly: CLSCompliant(false)]

namespace GtMotive.Fleet.FunctionalTests.Infrastructure
{
    public sealed class CompositionRootTestFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _database = new PostgreSqlBuilder()
            .WithImage("postgres:17.2-alpine")
            .Build();

        private ServiceProvider _serviceProvider;

        public IConfiguration Configuration { get; private set; }

        public async Task InitializeAsync()
        {
            await _database.StartAsync();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:FleetDb"] = _database.GetConnectionString(),
                })
                .Build();

            var services = new ServiceCollection();
            Configuration = configuration;
            ConfigureServices(services, configuration);
            services.AddSingleton<IConfiguration>(configuration);
            _serviceProvider = services.BuildServiceProvider();

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
            await context.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await _serviceProvider.DisposeAsync();
            await _database.DisposeAsync();
        }

        public async Task ResetDatabaseAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Rentals\", \"Vehicles\";");
        }

        public async Task UsingHandlerForRequest<TRequest>(Func<IRequestHandler<TRequest, Unit>, Task> handlerAction)
            where TRequest : IRequest
        {
            ArgumentNullException.ThrowIfNull(handlerAction);

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, Unit>>();

            await handlerAction.Invoke(handler);
        }

        public async Task UsingHandlerForRequestResponse<TRequest, TResponse>(Func<IRequestHandler<TRequest, TResponse>, Task> handlerAction)
            where TRequest : IRequest<TResponse>
        {
            ArgumentNullException.ThrowIfNull(handlerAction);

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

            if (handler == null)
            {
                Debug.Fail("The requested handler has not been registered");
            }

            await handlerAction.Invoke(handler);
        }

        public async Task UsingRepository<TRepository>(Func<TRepository, Task> handlerAction)
        {
            ArgumentNullException.ThrowIfNull(handlerAction);

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<TRepository>();

            if (handler == null)
            {
                Debug.Fail("The requested handler has not been registered");
            }

            await handlerAction.Invoke(handler);
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiDependencies();
            services.AddLogging();
            services.AddBaseInfrastructure(configuration);
        }
    }
}
