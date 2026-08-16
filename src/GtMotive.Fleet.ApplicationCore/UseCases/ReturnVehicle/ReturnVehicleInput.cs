using System;
using MediatR;

namespace GtMotive.Fleet.ApplicationCore.UseCases.ReturnVehicle
{
    /// <summary>
    /// Input message to return a rented vehicle.
    /// </summary>
    /// <param name="vehicleId">Identifier of the vehicle to return.</param>
    public sealed class ReturnVehicleInput(Guid vehicleId) : IRequest, IUseCaseInput
    {
        /// <summary>
        /// Gets the identifier of the vehicle to return.
        /// </summary>
        public Guid VehicleId { get; } = vehicleId;
    }
}
