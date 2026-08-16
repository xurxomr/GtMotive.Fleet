using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using GtMotive.Fleet.ApplicationCore.UseCases.RentVehicle;
using GtMotive.Fleet.Domain;
using GtMotive.Fleet.Domain.Vehicles;
using GtMotive.Fleet.FunctionalTests.Infrastructure;
using Xunit;

namespace GtMotive.Fleet.FunctionalTests.Specs
{
    public sealed class RentVehicleTests(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
    {
        private static readonly DateOnly ManufacturingDate = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-1);

        [Fact]
        public async Task RentVehicle_AvailableVehicle_MarksItRentedAndExcludesItFromAvailable()
        {
            // Arrange
            var vehicleId = await RegisterVehicleAsync();

            // Act
            await Fixture.UsingHandlerForRequest<RentVehicleInput>(handler =>
                handler.Handle(new RentVehicleInput(vehicleId, "renter-1"), CancellationToken.None));

            // Assert
            await Fixture.UsingRepository<IVehicleRepository>(async repository =>
            {
                var availableVehicles = await repository.GetAvailable();
                availableVehicles.Should().BeEmpty();
            });
        }

        [Fact]
        public async Task RentVehicle_RenterWithActiveRental_IsRejected()
        {
            // Arrange
            var firstVehicleId = await RegisterVehicleAsync();
            var secondVehicleId = await RegisterVehicleAsync();
            await Fixture.UsingHandlerForRequest<RentVehicleInput>(handler =>
                handler.Handle(new RentVehicleInput(firstVehicleId, "renter-1"), CancellationToken.None));

            // Act
            var act = () => Fixture.UsingHandlerForRequest<RentVehicleInput>(handler =>
                handler.Handle(new RentVehicleInput(secondVehicleId, "renter-1"), CancellationToken.None));

            // Assert
            await act.Should().ThrowAsync<DomainException>();
        }

        private async Task<Guid> RegisterVehicleAsync()
        {
            var licensePlate = Guid.NewGuid().ToString("N")[..10];
            await Fixture.UsingHandlerForRequest<RegisterVehicleInput>(handler =>
                handler.Handle(new RegisterVehicleInput(licensePlate, ManufacturingDate), CancellationToken.None));

            var vehicleId = Guid.Empty;
            await Fixture.UsingRepository<IVehicleRepository>(async repository =>
            {
                var vehicles = await repository.GetAvailable();
                vehicleId = vehicles.Single(vehicle => string.Equals(vehicle.LicensePlate.Value, licensePlate, StringComparison.OrdinalIgnoreCase)).Id;
            });

            return vehicleId;
        }
    }
}
