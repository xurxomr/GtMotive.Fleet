using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GtMotive.Fleet.Domain.Vehicles
{
    /// <summary>
    /// Persistence port for the <see cref="Vehicle"/> aggregate.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Adds a new vehicle to the fleet.
        /// </summary>
        /// <param name="vehicle">Vehicle to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Add(Vehicle vehicle);

        /// <summary>
        /// Gets the vehicles that are currently available to be rented.
        /// </summary>
        /// <returns>The available vehicles.</returns>
        Task<IReadOnlyList<Vehicle>> GetAvailable();

        /// <summary>
        /// Gets a vehicle by its identifier, or <c>null</c> when it does not exist.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <returns>The vehicle, or <c>null</c> when not found.</returns>
        Task<Vehicle> GetById(Guid id);
    }
}
