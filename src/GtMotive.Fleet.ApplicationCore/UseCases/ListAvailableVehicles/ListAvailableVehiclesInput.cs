using MediatR;

namespace GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles
{
    /// <summary>
    /// Input message to list the vehicles available in the fleet.
    /// </summary>
    public sealed class ListAvailableVehiclesInput : IRequest, IUseCaseInput
    {
    }
}
