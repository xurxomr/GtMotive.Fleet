using System;
using MediatR;

namespace GtMotive.Fleet.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Input message to rent a vehicle for a renter.
    /// </summary>
    /// <param name="vehicleId">Identifier of the vehicle to rent.</param>
    /// <param name="renterId">Identifier of the renter.</param>
    public sealed class RentVehicleInput(Guid vehicleId, string renterId) : IRequest, IUseCaseInput
    {
        /// <summary>
        /// Gets the identifier of the vehicle to rent.
        /// </summary>
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>
        /// Gets the identifier of the renter.
        /// </summary>
        public string RenterId { get; } = renterId;
    }
}
