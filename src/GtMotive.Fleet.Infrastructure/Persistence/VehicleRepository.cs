using System;
using System.Threading.Tasks;
using GtMotive.Fleet.Domain.Vehicles;

namespace GtMotive.Fleet.Infrastructure.Persistence
{
    public sealed class VehicleRepository(FleetDbContext context) : IVehicleRepository
    {
        public async Task Add(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            await context.Vehicles.AddAsync(vehicle);
        }
    }
}
