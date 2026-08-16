using System;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Fleet.ApplicationCore
{
    /// <summary>
    /// Adds Use Cases classes.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ApplicationConfiguration
    {
        /// <summary>
        /// Adds Use Cases to the ServiceCollection.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <returns>The modified instance.</returns>
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<RegisterVehicleUseCase>();
            services.AddScoped<IUseCase<RegisterVehicleInput>>(provider => provider.GetRequiredService<RegisterVehicleUseCase>());
            services.AddScoped<IRequestHandler<RegisterVehicleInput, Unit>>(provider => provider.GetRequiredService<RegisterVehicleUseCase>());

            services.AddScoped<ListAvailableVehiclesUseCase>();
            services.AddScoped<IUseCase<ListAvailableVehiclesInput>>(provider => provider.GetRequiredService<ListAvailableVehiclesUseCase>());
            services.AddScoped<IRequestHandler<ListAvailableVehiclesInput, Unit>>(provider => provider.GetRequiredService<ListAvailableVehiclesUseCase>());

            return services;
        }
    }
}
