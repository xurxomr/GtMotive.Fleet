using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using GtMotive.Fleet.Domain.Vehicles;
using GtMotive.Fleet.FunctionalTests.Infrastructure;
using Xunit;

namespace GtMotive.Fleet.FunctionalTests.Specs
{
    public sealed class ListAvailableVehiclesTests(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
    {
        [Fact]
        public async Task GetAvailable_WithRegisteredVehicles_ReturnsThem()
        {
            // Arrange
            var manufacturingDate = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-1);
            await Fixture.UsingHandlerForRequest<RegisterVehicleInput>(handler =>
                handler.Handle(new RegisterVehicleInput(Guid.NewGuid().ToString("N")[..10], manufacturingDate), CancellationToken.None));
            await Fixture.UsingHandlerForRequest<RegisterVehicleInput>(handler =>
                handler.Handle(new RegisterVehicleInput(Guid.NewGuid().ToString("N")[..10], manufacturingDate), CancellationToken.None));

            // Act & Assert
            await Fixture.UsingRepository<IVehicleRepository>(async repository =>
            {
                var availableVehicles = await repository.GetAvailable();
                availableVehicles.Should().HaveCount(2);
            });
        }
    }
}
