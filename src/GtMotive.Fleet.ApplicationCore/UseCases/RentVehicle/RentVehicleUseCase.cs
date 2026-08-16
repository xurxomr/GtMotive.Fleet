using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Fleet.Domain;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Domain.Rentals;
using GtMotive.Fleet.Domain.Vehicles;
using MediatR;

namespace GtMotive.Fleet.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Interactor that rents a vehicle for a renter.
    /// </summary>
    /// <param name="vehicleRepository">Repository used to load and persist the vehicle.</param>
    /// <param name="rentalRepository">Repository used to enforce the single active rental rule.</param>
    /// <param name="unitOfWork">Unit of work used to commit the changes.</param>
    /// <param name="outputPort">Output port used to present the result.</param>
    public sealed class RentVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IRentalRepository rentalRepository,
        IUnitOfWork unitOfWork,
        IOutputPortStandard<RentVehicleOutput> outputPort)
        : IUseCase<RentVehicleInput>, IRequestHandler<RentVehicleInput, Unit>
    {
        /// <inheritdoc />
        public async Task Execute(RentVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var renterId = RenterId.Create(input.RenterId);
            if (await rentalRepository.HasActiveRental(renterId))
            {
                throw new DomainException("The renter already has an active rental.");
            }

            var vehicle = await vehicleRepository.GetById(input.VehicleId);
            if (vehicle is null)
            {
                throw new DomainException("The vehicle does not exist.");
            }

            vehicle.Rent();

            var rental = Rental.Create(vehicle.Id, renterId, DateOnly.FromDateTime(DateTime.UtcNow));
            await rentalRepository.Add(rental);
            await unitOfWork.Save();

            outputPort.StandardHandle(new RentVehicleOutput(rental.Id, vehicle.Id, renterId.Value, rental.StartedOn));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(RentVehicleInput request, CancellationToken cancellationToken)
        {
            await Execute(request);
            return Unit.Value;
        }
    }
}
