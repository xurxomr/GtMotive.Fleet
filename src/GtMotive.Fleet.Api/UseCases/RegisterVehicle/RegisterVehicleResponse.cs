using System;

namespace GtMotive.Fleet.Api.UseCases.RegisterVehicle
{
    public sealed record RegisterVehicleResponse(Guid Id, string LicensePlate, DateOnly ManufacturingDate);
}
