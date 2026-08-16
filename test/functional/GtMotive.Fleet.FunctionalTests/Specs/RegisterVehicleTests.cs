using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using GtMotive.Fleet.FunctionalTests.Infrastructure;
using GtMotive.Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GtMotive.Fleet.FunctionalTests.Specs
{
    public sealed class RegisterVehicleTests(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
    {
        [Fact]
        public async Task Handle_ValidInput_PersistsVehicleInDatabase()
        {
            // Arrange
            var licensePlate = Guid.NewGuid().ToString("N")[..12];
            var input = new RegisterVehicleInput(licensePlate, DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-1));

            // Act
            await Fixture.UsingHandlerForRequest<RegisterVehicleInput>(handler => handler.Handle(input, CancellationToken.None));

            // Assert
            await Fixture.UsingRepository<FleetDbContext>(async context =>
            {
                var storedVehicles = await context.Vehicles.ToListAsync();
                storedVehicles.Should().ContainSingle(vehicle => vehicle.LicensePlate.Value.Equals(licensePlate.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            });
        }
    }
}
