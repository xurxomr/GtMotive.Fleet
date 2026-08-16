using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using GtMotive.Fleet.Domain;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Domain.Vehicles;
using Moq;
using Xunit;

namespace GtMotive.Fleet.UnitTests.ApplicationCore.UseCases.RegisterVehicle
{
    public sealed class RegisterVehicleUseCaseTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IOutputPortStandard<RegisterVehicleOutput>> _outputPort = new();

        [Fact]
        public async Task Execute_ValidInput_AddsVehiclePersistsAndInvokesOutputPort()
        {
            // Arrange
            var useCase = new RegisterVehicleUseCase(_vehicleRepository.Object, _unitOfWork.Object, _outputPort.Object);
            var input = new RegisterVehicleInput("1234ABC", DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-1));

            // Act
            await useCase.Execute(input);

            // Assert
            _vehicleRepository.Verify(repository => repository.Add(It.IsAny<Vehicle>()), Times.Once);
            _unitOfWork.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
            _outputPort.Verify(outputPort => outputPort.StandardHandle(It.IsAny<RegisterVehicleOutput>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ManufacturingDateOlderThanAgeLimit_ThrowsDomainExceptionAndDoesNotPersist()
        {
            // Arrange
            var useCase = new RegisterVehicleUseCase(_vehicleRepository.Object, _unitOfWork.Object, _outputPort.Object);
            var manufacturingDate = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-(Vehicle.MaxManufacturingAgeInYears + 1));
            var input = new RegisterVehicleInput("1234ABC", manufacturingDate);

            // Act
            var act = async () => await useCase.Execute(input);

            // Assert
            await act.Should().ThrowAsync<DomainException>();
            _vehicleRepository.Verify(repository => repository.Add(It.IsAny<Vehicle>()), Times.Never);
            _unitOfWork.Verify(unitOfWork => unitOfWork.Save(), Times.Never);
        }
    }
}
