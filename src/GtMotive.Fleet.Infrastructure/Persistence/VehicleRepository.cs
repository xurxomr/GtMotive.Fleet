using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Fleet.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace GtMotive.Fleet.Infrastructure.Persistence
{
    public sealed class VehicleRepository(FleetDbContext context) : IVehicleRepository
    {
        public async Task Add(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            await context.Vehicles.AddAsync(vehicle);
        }

        public async Task<IReadOnlyList<Vehicle>> GetAvailable()
        {
            return await context.Vehicles
                .Where(vehicle => vehicle.Status == VehicleStatus.Available)
                .ToListAsync();
        }
    }
}
