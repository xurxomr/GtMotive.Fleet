using System;

namespace GtMotive.Fleet.ApplicationCore.UseCases.ReturnVehicle
{
    /// <summary>
    /// Output message returned after a vehicle is returned.
    /// </summary>
    public sealed class ReturnVehicleOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnVehicleOutput"/> class.
        /// </summary>
        /// <param name="vehicleId">Identifier of the returned vehicle.</param>
        /// <param name="rentalId">Identifier of the closed rental.</param>
        /// <param name="endedOn">Date on which the rental was closed.</param>
        public ReturnVehicleOutput(Guid vehicleId, Guid rentalId, DateOnly endedOn)
        {
            VehicleId = vehicleId;
            RentalId = rentalId;
            EndedOn = endedOn;
        }

        /// <summary>
        /// Gets the identifier of the returned vehicle.
        /// </summary>
        public Guid VehicleId { get; }

        /// <summary>
        /// Gets the identifier of the closed rental.
        /// </summary>
        public Guid RentalId { get; }

        /// <summary>
        /// Gets the date on which the rental was closed.
        /// </summary>
        public DateOnly EndedOn { get; }
    }
}
