using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Fleet.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Fleet.InfrastructureTests.Specs
{
    public sealed class RegisterVehicleControllerTests(GenericInfrastructureTestServerFixture fixture) : InfrastructureTestBase(fixture)
    {
        [Fact]
        public async Task Post_RequestWithoutLicensePlate_ReturnsBadRequest()
        {
            // Arrange
            using var client = Fixture.Server.CreateClient();
            var payload = new { manufacturingDate = "2023-01-01" };

            // Act
            using var response = await client.PostAsJsonAsync("/api/vehicles", payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
