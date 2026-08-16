using System;
using System.Threading.Tasks;
using GtMotive.Fleet.Domain.Rentals;
using Microsoft.EntityFrameworkCore;

namespace GtMotive.Fleet.Infrastructure.Persistence
{
    public sealed class RentalRepository(FleetDbContext context) : IRentalRepository
    {
        public async Task Add(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            await context.Rentals.AddAsync(rental);
        }

        public async Task<bool> HasActiveRental(RenterId renterId)
        {
            ArgumentNullException.ThrowIfNull(renterId);

            return await context.Rentals.AnyAsync(rental =>
                rental.RenterId == renterId && rental.Status == RentalStatus.Active);
        }

        public async Task<Rental> GetActiveByVehicle(Guid vehicleId)
        {
            return await context.Rentals.FirstOrDefaultAsync(rental =>
                rental.VehicleId == vehicleId && rental.Status == RentalStatus.Active);
        }
    }
}
