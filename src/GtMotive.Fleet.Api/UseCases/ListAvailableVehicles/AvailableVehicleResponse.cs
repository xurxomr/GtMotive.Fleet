using System;

namespace GtMotive.Fleet.Api.UseCases.ListAvailableVehicles
{
    public sealed record AvailableVehicleResponse(Guid Id, string LicensePlate, DateOnly ManufacturingDate);
}
