using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.ReturnVehicle;
using GtMotive.Fleet.Domain;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Domain.Rentals;
using GtMotive.Fleet.Domain.Vehicles;
using Moq;
using Xunit;

namespace GtMotive.Fleet.UnitTests.ApplicationCore.UseCases.ReturnVehicle
{
    public sealed class ReturnVehicleUseCaseTests
    {
        private static readonly DateOnly Today = new(2026, 8, 16);

        private readonly Mock<IVehicleRepository> _vehicleRepository = new();
        private readonly Mock<IRentalRepository> _rentalRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IOutputPortStandard<ReturnVehicleOutput>> _outputPort = new();

        [Fact]
        public async Task Execute_RentedVehicleWithActiveRental_ReturnsVehicleAndClosesRental()
        {
            // Arrange
            var vehicle = Vehicle.Create(LicensePlate.Create("1234ABC"), Today.AddYears(-1), Today);
            vehicle.Rent();
            var rental = Rental.Create(vehicle.Id, RenterId.Create("renter-1"), Today);
            _vehicleRepository.Setup(repository => repository.GetById(vehicle.Id)).ReturnsAsync(vehicle);
            _rentalRepository.Setup(repository => repository.GetActiveByVehicle(vehicle.Id)).ReturnsAsync(rental);
            var useCase = new ReturnVehicleUseCase(_vehicleRepository.Object, _rentalRepository.Object, _unitOfWork.Object, _outputPort.Object);

            // Act
            await useCase.Execute(new ReturnVehicleInput(vehicle.Id));

            // Assert
            vehicle.Status.Should().Be(VehicleStatus.Available);
            rental.Status.Should().Be(RentalStatus.Closed);
            _unitOfWork.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
            _outputPort.Verify(outputPort => outputPort.StandardHandle(It.IsAny<ReturnVehicleOutput>()), Times.Once);
        }

        [Fact]
        public async Task Execute_VehicleWithoutActiveRental_ThrowsDomainExceptionAndDoesNotPersist()
        {
            // Arrange
            var vehicle = Vehicle.Create(LicensePlate.Create("1234ABC"), Today.AddYears(-1), Today);
            _vehicleRepository.Setup(repository => repository.GetById(vehicle.Id)).ReturnsAsync(vehicle);
            _rentalRepository.Setup(repository => repository.GetActiveByVehicle(vehicle.Id)).ReturnsAsync((Rental)null);
            var useCase = new ReturnVehicleUseCase(_vehicleRepository.Object, _rentalRepository.Object, _unitOfWork.Object, _outputPort.Object);

            // Act
            var act = async () => await useCase.Execute(new ReturnVehicleInput(vehicle.Id));

            // Assert
            await act.Should().ThrowAsync<DomainException>();
            _unitOfWork.Verify(unitOfWork => unitOfWork.Save(), Times.Never);
        }
    }
}
