using System;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Infrastructure.Interfaces;
using GtMotive.Fleet.Infrastructure.Logging;
using GtMotive.Fleet.Infrastructure.Telemetry;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Fleet.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        [ExcludeFromCodeCoverage]
        public static IInfrastructureBuilder AddBaseInfrastructure(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<ITelemetry, NoOpTelemetry>();

            return new InfrastructureBuilder(services);
        }

        private sealed class InfrastructureBuilder(IServiceCollection services) : IInfrastructureBuilder
        {
            public IServiceCollection Services { get; } = services;
        }
    }
}
