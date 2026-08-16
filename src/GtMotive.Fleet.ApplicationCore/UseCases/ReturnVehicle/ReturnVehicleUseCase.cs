using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Fleet.Domain;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Domain.Rentals;
using GtMotive.Fleet.Domain.Vehicles;
using MediatR;

namespace GtMotive.Fleet.ApplicationCore.UseCases.ReturnVehicle
{
    /// <summary>
    /// Interactor that returns a rented vehicle and closes its active rental.
    /// </summary>
    /// <param name="vehicleRepository">Repository used to load and persist the vehicle.</param>
    /// <param name="rentalRepository">Repository used to load the active rental.</param>
    /// <param name="unitOfWork">Unit of work used to commit the changes.</param>
    /// <param name="outputPort">Output port used to present the result.</param>
    public sealed class ReturnVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IRentalRepository rentalRepository,
        IUnitOfWork unitOfWork,
        IOutputPortStandard<ReturnVehicleOutput> outputPort)
        : IUseCase<ReturnVehicleInput>, IRequestHandler<ReturnVehicleInput, Unit>
    {
        /// <inheritdoc />
        public async Task Execute(ReturnVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var vehicle = await vehicleRepository.GetById(input.VehicleId);
            if (vehicle is null)
            {
                throw new DomainException("The vehicle does not exist.");
            }

            var rental = await rentalRepository.GetActiveByVehicle(input.VehicleId);
            if (rental is null)
            {
                throw new DomainException("The vehicle is not rented.");
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            rental.Close(today);
            vehicle.Return();
            await unitOfWork.Save();

            outputPort.StandardHandle(new ReturnVehicleOutput(vehicle.Id, rental.Id, today));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(ReturnVehicleInput request, CancellationToken cancellationToken)
        {
            await Execute(request);
            return Unit.Value;
        }
    }
}
