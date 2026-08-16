using System;

namespace GtMotive.Fleet.Api.UseCases.RentVehicle
{
    public sealed record RentVehicleResponse(Guid RentalId, Guid VehicleId, string RenterId, DateOnly StartedOn);
}
