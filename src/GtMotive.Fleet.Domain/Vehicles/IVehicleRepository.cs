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
    }
}
