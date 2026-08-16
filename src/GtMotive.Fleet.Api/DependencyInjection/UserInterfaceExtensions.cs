using GtMotive.Fleet.Api.UseCases.ListAvailableVehicles;
using GtMotive.Fleet.Api.UseCases.RegisterVehicle;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Fleet.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<RegisterVehiclePresenter>();
            services.AddScoped<IOutputPortStandard<RegisterVehicleOutput>>(provider => provider.GetRequiredService<RegisterVehiclePresenter>());

            services.AddScoped<ListAvailableVehiclesPresenter>();
            services.AddScoped<IOutputPortStandard<ListAvailableVehiclesOutput>>(provider => provider.GetRequiredService<ListAvailableVehiclesPresenter>());

            return services;
        }
    }
}
