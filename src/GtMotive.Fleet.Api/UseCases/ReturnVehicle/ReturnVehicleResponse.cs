using System;

namespace GtMotive.Fleet.Api.UseCases.ReturnVehicle
{
    public sealed record ReturnVehicleResponse(Guid VehicleId, Guid RentalId, DateOnly EndedOn);
}
