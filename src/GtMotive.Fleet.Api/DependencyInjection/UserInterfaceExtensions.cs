using GtMotive.Fleet.Api.UseCases.ListAvailableVehicles;
using GtMotive.Fleet.Api.UseCases.RegisterVehicle;
using GtMotive.Fleet.Api.UseCases.RentVehicle;
using GtMotive.Fleet.Api.UseCases.ReturnVehicle;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using GtMotive.Fleet.ApplicationCore.UseCases.RentVehicle;
using GtMotive.Fleet.ApplicationCore.UseCases.ReturnVehicle;
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

            services.AddScoped<RentVehiclePresenter>();
            services.AddScoped<IOutputPortStandard<RentVehicleOutput>>(provider => provider.GetRequiredService<RentVehiclePresenter>());

            services.AddScoped<ReturnVehiclePresenter>();
            services.AddScoped<IOutputPortStandard<ReturnVehicleOutput>>(provider => provider.GetRequiredService<ReturnVehiclePresenter>());

            return services;
        }
    }
}
