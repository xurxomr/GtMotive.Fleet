using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles;
using GtMotive.Fleet.Domain.Vehicles;
using Moq;
using Xunit;

namespace GtMotive.Fleet.UnitTests.ApplicationCore.UseCases.ListAvailableVehicles
{
    public sealed class ListAvailableVehiclesUseCaseTests
    {
        private static readonly DateOnly Today = new(2026, 8, 16);

        private readonly Mock<IVehicleRepository> _vehicleRepository = new();
        private readonly Mock<IOutputPortStandard<ListAvailableVehiclesOutput>> _outputPort = new();

        [Fact]
        public async Task Execute_WithAvailableVehicles_PassesMappedVehiclesToOutputPort()
        {
            // Arrange
            var vehicles = new List<Vehicle>
            {
                Vehicle.Create(LicensePlate.Create("1234ABC"), Today.AddYears(-1), Today),
                Vehicle.Create(LicensePlate.Create("5678DEF"), Today.AddYears(-2), Today),
            };
            _vehicleRepository.Setup(repository => repository.GetAvailable()).ReturnsAsync(vehicles);
            var useCase = new ListAvailableVehiclesUseCase(_vehicleRepository.Object, _outputPort.Object);

            // Act
            await useCase.Execute(new ListAvailableVehiclesInput());

            // Assert
            _outputPort.Verify(
                outputPort => outputPort.StandardHandle(It.Is<ListAvailableVehiclesOutput>(output => output.Vehicles.Count == 2)),
                Times.Once);
        }
    }
}
