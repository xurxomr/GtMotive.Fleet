using System;

namespace GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle
{
    /// <summary>
    /// Output message returned after a vehicle is registered in the fleet.
    /// </summary>
    public sealed class RegisterVehicleOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterVehicleOutput"/> class.
        /// </summary>
        /// <param name="id">Identifier of the registered vehicle.</param>
        /// <param name="licensePlate">License plate of the registered vehicle.</param>
        /// <param name="manufacturingDate">Manufacturing date of the registered vehicle.</param>
        public RegisterVehicleOutput(Guid id, string licensePlate, DateOnly manufacturingDate)
        {
            Id = id;
            LicensePlate = licensePlate;
            ManufacturingDate = manufacturingDate;
        }

        /// <summary>
        /// Gets the identifier of the registered vehicle.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the license plate of the registered vehicle.
        /// </summary>
        public string LicensePlate { get; }

        /// <summary>
        /// Gets the manufacturing date of the registered vehicle.
        /// </summary>
        public DateOnly ManufacturingDate { get; }
    }
}
