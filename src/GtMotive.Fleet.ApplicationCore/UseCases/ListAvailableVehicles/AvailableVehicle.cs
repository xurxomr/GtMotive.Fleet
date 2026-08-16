using System;

namespace GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles
{
    /// <summary>
    /// Represents an available vehicle in the fleet listing.
    /// </summary>
    /// <param name="Id">Vehicle identifier.</param>
    /// <param name="LicensePlate">Vehicle license plate.</param>
    /// <param name="ManufacturingDate">Vehicle manufacturing date.</param>
    public sealed record AvailableVehicle(Guid Id, string LicensePlate, DateOnly ManufacturingDate);
}
