using System.Diagnostics.CodeAnalysis;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Fleet.Infrastructure.Persistence.DependencyInjection
{
    public static class PersistenceExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FleetDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<FleetDbContext>());
            services.AddScoped<IVehicleRepository, VehicleRepository>();

            return services;
        }
    }
}
