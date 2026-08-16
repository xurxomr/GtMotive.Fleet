using System;

namespace GtMotive.Fleet.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Output message returned after a vehicle is rented.
    /// </summary>
    public sealed class RentVehicleOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentVehicleOutput"/> class.
        /// </summary>
        /// <param name="rentalId">Identifier of the created rental.</param>
        /// <param name="vehicleId">Identifier of the rented vehicle.</param>
        /// <param name="renterId">Identifier of the renter.</param>
        /// <param name="startedOn">Date on which the rental started.</param>
        public RentVehicleOutput(Guid rentalId, Guid vehicleId, string renterId, DateOnly startedOn)
        {
            RentalId = rentalId;
            VehicleId = vehicleId;
            RenterId = renterId;
            StartedOn = startedOn;
        }

        /// <summary>
        /// Gets the identifier of the created rental.
        /// </summary>
        public Guid RentalId { get; }

        /// <summary>
        /// Gets the identifier of the rented vehicle.
        /// </summary>
        public Guid VehicleId { get; }

        /// <summary>
        /// Gets the identifier of the renter.
        /// </summary>
        public string RenterId { get; }

        /// <summary>
        /// Gets the date on which the rental started.
        /// </summary>
        public DateOnly StartedOn { get; }
    }
}
