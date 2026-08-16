using System.Collections.Generic;

namespace GtMotive.Fleet.Api.UseCases.ListAvailableVehicles
{
    public sealed record ListAvailableVehiclesResponse(IReadOnlyList<AvailableVehicleResponse> Vehicles);
}
