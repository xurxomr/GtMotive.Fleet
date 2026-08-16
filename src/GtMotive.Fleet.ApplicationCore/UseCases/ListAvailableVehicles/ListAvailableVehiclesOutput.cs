using System.Collections.Generic;

namespace GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles
{
    /// <summary>
    /// Output message with the vehicles available in the fleet.
    /// </summary>
    public sealed class ListAvailableVehiclesOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListAvailableVehiclesOutput"/> class.
        /// </summary>
        /// <param name="vehicles">Available vehicles.</param>
        public ListAvailableVehiclesOutput(IReadOnlyList<AvailableVehicle> vehicles)
        {
            Vehicles = vehicles;
        }

        /// <summary>
        /// Gets the available vehicles.
        /// </summary>
        public IReadOnlyList<AvailableVehicle> Vehicles { get; }
    }
}
