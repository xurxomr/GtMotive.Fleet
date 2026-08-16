using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Fleet.Domain.Vehicles;
using MediatR;

namespace GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles
{
    /// <summary>
    /// Interactor that lists the vehicles available in the fleet.
    /// </summary>
    /// <param name="vehicleRepository">Repository used to query the vehicles.</param>
    /// <param name="outputPort">Output port used to present the result.</param>
    public sealed class ListAvailableVehiclesUseCase(
        IVehicleRepository vehicleRepository,
        IOutputPortStandard<ListAvailableVehiclesOutput> outputPort)
        : IUseCase<ListAvailableVehiclesInput>, IRequestHandler<ListAvailableVehiclesInput, Unit>
    {
        /// <inheritdoc />
        public async Task Execute(ListAvailableVehiclesInput input)
        {
            var vehicles = await vehicleRepository.GetAvailable();
            var availableVehicles = vehicles
                .Select(vehicle => new AvailableVehicle(vehicle.Id, vehicle.LicensePlate.Value, vehicle.ManufacturingDate))
                .ToList();

            outputPort.StandardHandle(new ListAvailableVehiclesOutput(availableVehicles));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(ListAvailableVehiclesInput request, CancellationToken cancellationToken)
        {
            await Execute(request);
            return Unit.Value;
        }
    }
}
