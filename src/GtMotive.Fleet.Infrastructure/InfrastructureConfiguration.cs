using System;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Infrastructure.Interfaces;
using GtMotive.Fleet.Infrastructure.Logging;
using GtMotive.Fleet.Infrastructure.Persistence.DependencyInjection;
using GtMotive.Fleet.Infrastructure.Telemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Fleet.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        [ExcludeFromCodeCoverage]
        public static IInfrastructureBuilder AddBaseInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<ITelemetry, NoOpTelemetry>();
            services.AddPersistence(configuration.GetConnectionString("FleetDb"));

            return new InfrastructureBuilder(services);
        }

        private sealed class InfrastructureBuilder(IServiceCollection services) : IInfrastructureBuilder
        {
            public IServiceCollection Services { get; } = services;
        }
    }
}
