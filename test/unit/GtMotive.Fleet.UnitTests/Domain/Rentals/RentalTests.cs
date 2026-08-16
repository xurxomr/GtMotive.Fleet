using System;
using FluentAssertions;
using GtMotive.Fleet.Domain.Rentals;
using Xunit;

namespace GtMotive.Fleet.UnitTests.Domain.Rentals
{
    public sealed class RentalTests
    {
        private static readonly DateOnly Today = new(2026, 8, 16);

        [Fact]
        public void Create_WithValidData_ReturnsActiveRental()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var renterId = RenterId.Create("renter-1");

            // Act
            var rental = Rental.Create(vehicleId, renterId, Today);

            // Assert
            rental.Id.Should().NotBeEmpty();
            rental.VehicleId.Should().Be(vehicleId);
            rental.RenterId.Should().Be(renterId);
            rental.StartedOn.Should().Be(Today);
            rental.Status.Should().Be(RentalStatus.Active);
        }
    }
}
