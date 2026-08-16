using System;
using FluentAssertions;
using GtMotive.Fleet.Domain;
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
            rental.EndedOn.Should().BeNull();
        }

        [Fact]
        public void Close_ActiveRental_MarksItClosedWithEndDate()
        {
            // Arrange
            var rental = Rental.Create(Guid.NewGuid(), RenterId.Create("renter-1"), Today);

            // Act
            rental.Close(Today);

            // Assert
            rental.Status.Should().Be(RentalStatus.Closed);
            rental.EndedOn.Should().Be(Today);
        }

        [Fact]
        public void Close_AlreadyClosedRental_ThrowsDomainException()
        {
            // Arrange
            var rental = Rental.Create(Guid.NewGuid(), RenterId.Create("renter-1"), Today);
            rental.Close(Today);

            // Act
            var act = () => rental.Close(Today);

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
