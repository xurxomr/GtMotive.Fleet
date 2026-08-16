using System;
using System.Threading.Tasks;

namespace GtMotive.Fleet.Domain.Rentals
{
    /// <summary>
    /// Persistence port for the <see cref="Rental"/> aggregate.
    /// </summary>
    public interface IRentalRepository
    {
        /// <summary>
        /// Adds a new rental.
        /// </summary>
        /// <param name="rental">Rental to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Add(Rental rental);

        /// <summary>
        /// Determines whether the renter already has an active rental.
        /// </summary>
        /// <param name="renterId">Identifier of the renter.</param>
        /// <returns><c>true</c> if the renter has an active rental; otherwise, <c>false</c>.</returns>
        Task<bool> HasActiveRental(RenterId renterId);

        /// <summary>
        /// Gets the active rental for a vehicle, or <c>null</c> when it is not rented.
        /// </summary>
        /// <param name="vehicleId">Identifier of the vehicle.</param>
        /// <returns>The active rental, or <c>null</c> when the vehicle is not rented.</returns>
        Task<Rental> GetActiveByVehicle(Guid vehicleId);
    }
}
