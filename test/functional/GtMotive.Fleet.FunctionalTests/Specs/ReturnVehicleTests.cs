using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using GtMotive.Fleet.ApplicationCore.UseCases.RentVehicle;
using GtMotive.Fleet.ApplicationCore.UseCases.ReturnVehicle;
using GtMotive.Fleet.Domain.Vehicles;
using GtMotive.Fleet.FunctionalTests.Infrastructure;
using Xunit;

namespace GtMotive.Fleet.FunctionalTests.Specs
{
    public sealed class ReturnVehicleTests(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
    {
        private static readonly DateOnly ManufacturingDate = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-1);

        [Fact]
        public async Task ReturnVehicle_RentedVehicle_MakesItAvailableAgain()
        {
            // Arrange
            var vehicleId = await RegisterAndRentAsync("renter-1");

            // Act
            await Fixture.UsingHandlerForRequest<ReturnVehicleInput>(handler =>
                handler.Handle(new ReturnVehicleInput(vehicleId), CancellationToken.None));

            // Assert
            await Fixture.UsingRepository<IVehicleRepository>(async repository =>
            {
                var availableVehicles = await repository.GetAvailable();
                availableVehicles.Should().ContainSingle(vehicle => vehicle.Id == vehicleId);
            });
        }

        private async Task<Guid> RegisterAndRentAsync(string renterId)
        {
            var licensePlate = Guid.NewGuid().ToString("N")[..10];
            await Fixture.UsingHandlerForRequest<RegisterVehicleInput>(handler =>
                handler.Handle(new RegisterVehicleInput(licensePlate, ManufacturingDate), CancellationToken.None));

            var vehicleId = Guid.Empty;
            await Fixture.UsingRepository<IVehicleRepository>(async repository =>
            {
                var vehicles = await repository.GetAvailable();
                vehicleId = vehicles.Single().Id;
            });

            await Fixture.UsingHandlerForRequest<RentVehicleInput>(handler =>
                handler.Handle(new RentVehicleInput(vehicleId, renterId), CancellationToken.None));

            return vehicleId;
        }
    }
}
