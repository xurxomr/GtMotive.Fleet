using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Domain.Vehicles;
using MediatR;

namespace GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle
{
    /// <summary>
    /// Interactor that registers a new vehicle in the fleet.
    /// </summary>
    /// <param name="vehicleRepository">Repository used to persist the vehicle.</param>
    /// <param name="unitOfWork">Unit of work used to commit the changes.</param>
    /// <param name="outputPort">Output port used to present the result.</param>
    public sealed class RegisterVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        IOutputPortStandard<RegisterVehicleOutput> outputPort)
        : IUseCase<RegisterVehicleInput>, IRequestHandler<RegisterVehicleInput, Unit>
    {
        /// <inheritdoc />
        public async Task Execute(RegisterVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var licensePlate = LicensePlate.Create(input.LicensePlate);
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var vehicle = Vehicle.Create(licensePlate, input.ManufacturingDate, today);

            await vehicleRepository.Add(vehicle);
            await unitOfWork.Save();

            outputPort.StandardHandle(new RegisterVehicleOutput(vehicle.Id, vehicle.LicensePlate.Value, vehicle.ManufacturingDate));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(RegisterVehicleInput request, CancellationToken cancellationToken)
        {
            await Execute(request);
            return Unit.Value;
        }
    }
}
