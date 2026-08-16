using System;
using MediatR;

namespace GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle
{
    /// <summary>
    /// Input message to register a new vehicle in the fleet.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RegisterVehicleInput"/> class.
    /// </remarks>
    /// <param name="licensePlate">License plate of the vehicle to register.</param>
    /// <param name="manufacturingDate">Manufacturing date of the vehicle to register.</param>
    public sealed class RegisterVehicleInput(
        string licensePlate,
        DateOnly manufacturingDate)
        : IRequest, IUseCaseInput
    {
        /// <summary>
        /// Gets the license plate of the vehicle to register.
        /// </summary>
        public string LicensePlate { get; } = licensePlate;

        /// <summary>
        /// Gets the manufacturing date of the vehicle to register.
        /// </summary>
        public DateOnly ManufacturingDate { get; } = manufacturingDate;
    }
}
