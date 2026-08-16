using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.RentVehicle;
using GtMotive.Fleet.Domain;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Domain.Rentals;
using GtMotive.Fleet.Domain.Vehicles;
using Moq;
using Xunit;

namespace GtMotive.Fleet.UnitTests.ApplicationCore.UseCases.RentVehicle
{
    public sealed class RentVehicleUseCaseTests
    {
        private static readonly DateOnly Today = new(2026, 8, 16);

        private readonly Mock<IVehicleRepository> _vehicleRepository = new();
        private readonly Mock<IRentalRepository> _rentalRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IOutputPortStandard<RentVehicleOutput>> _outputPort = new();

        [Fact]
        public async Task Execute_AvailableVehicleAndRenterWithoutActiveRental_RentsVehicleAndPersists()
        {
            // Arrange
            var vehicle = Vehicle.Create(LicensePlate.Create("1234ABC"), Today.AddYears(-1), Today);
            _rentalRepository.Setup(repository => repository.HasActiveRental(It.IsAny<RenterId>())).ReturnsAsync(false);
            _vehicleRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(vehicle);
            var useCase = new RentVehicleUseCase(_vehicleRepository.Object, _rentalRepository.Object, _unitOfWork.Object, _outputPort.Object);

            // Act
            await useCase.Execute(new RentVehicleInput(vehicle.Id, "renter-1"));

            // Assert
            vehicle.Status.Should().Be(VehicleStatus.Rented);
            _rentalRepository.Verify(repository => repository.Add(It.IsAny<Rental>()), Times.Once);
            _unitOfWork.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
            _outputPort.Verify(outputPort => outputPort.StandardHandle(It.IsAny<RentVehicleOutput>()), Times.Once);
        }

        [Fact]
        public async Task Execute_RenterWithActiveRental_ThrowsDomainExceptionAndDoesNotPersist()
        {
            // Arrange
            _rentalRepository.Setup(repository => repository.HasActiveRental(It.IsAny<RenterId>())).ReturnsAsync(true);
            var useCase = new RentVehicleUseCase(_vehicleRepository.Object, _rentalRepository.Object, _unitOfWork.Object, _outputPort.Object);

            // Act
            var act = async () => await useCase.Execute(new RentVehicleInput(Guid.NewGuid(), "renter-1"));

            // Assert
            await act.Should().ThrowAsync<DomainException>();
            _rentalRepository.Verify(repository => repository.Add(It.IsAny<Rental>()), Times.Never);
            _unitOfWork.Verify(unitOfWork => unitOfWork.Save(), Times.Never);
        }

        [Fact]
        public async Task Execute_NonExistentVehicle_ThrowsDomainExceptionAndDoesNotPersist()
        {
            // Arrange
            _rentalRepository.Setup(repository => repository.HasActiveRental(It.IsAny<RenterId>())).ReturnsAsync(false);
            _vehicleRepository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync((Vehicle)null);
            var useCase = new RentVehicleUseCase(_vehicleRepository.Object, _rentalRepository.Object, _unitOfWork.Object, _outputPort.Object);

            // Act
            var act = async () => await useCase.Execute(new RentVehicleInput(Guid.NewGuid(), "renter-1"));

            // Assert
            await act.Should().ThrowAsync<DomainException>();
            _rentalRepository.Verify(repository => repository.Add(It.IsAny<Rental>()), Times.Never);
            _unitOfWork.Verify(unitOfWork => unitOfWork.Save(), Times.Never);
        }
    }
}
