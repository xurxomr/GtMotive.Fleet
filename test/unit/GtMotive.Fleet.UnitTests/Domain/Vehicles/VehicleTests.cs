using System;
using FluentAssertions;
using GtMotive.Fleet.Domain;
using GtMotive.Fleet.Domain.Vehicles;
using Xunit;

namespace GtMotive.Fleet.UnitTests.Domain.Vehicles
{
    public sealed class VehicleTests
    {
        private static readonly DateOnly Today = new(2026, 8, 16);

        [Fact]
        public void Create_ManufacturingDateWithinAgeLimit_ReturnsAvailableVehicle()
        {
            // Arrange
            var licensePlate = LicensePlate.Create("1234ABC");
            var manufacturingDate = Today.AddYears(-2);

            // Act
            var vehicle = Vehicle.Create(licensePlate, manufacturingDate, Today);

            // Assert
            vehicle.Id.Should().NotBeEmpty();
            vehicle.LicensePlate.Should().Be(licensePlate);
            vehicle.ManufacturingDate.Should().Be(manufacturingDate);
            vehicle.Status.Should().Be(VehicleStatus.Available);
        }

        [Fact]
        public void Create_ManufacturingDateAtAgeLimit_DoesNotThrow()
        {
            // Arrange
            var licensePlate = LicensePlate.Create("1234ABC");
            var manufacturingDate = Today.AddYears(-Vehicle.MaxManufacturingAgeInYears);

            // Act
            var act = () => Vehicle.Create(licensePlate, manufacturingDate, Today);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Create_ManufacturingDateOlderThanAgeLimit_ThrowsDomainException()
        {
            // Arrange
            var licensePlate = LicensePlate.Create("1234ABC");
            var manufacturingDate = Today.AddYears(-Vehicle.MaxManufacturingAgeInYears).AddDays(-1);

            // Act
            var act = () => Vehicle.Create(licensePlate, manufacturingDate, Today);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Rent_AvailableVehicle_MarksItAsRented()
        {
            // Arrange
            var vehicle = Vehicle.Create(LicensePlate.Create("1234ABC"), Today.AddYears(-1), Today);

            // Act
            vehicle.Rent();

            // Assert
            vehicle.Status.Should().Be(VehicleStatus.Rented);
        }

        [Fact]
        public void Rent_AlreadyRentedVehicle_ThrowsDomainException()
        {
            // Arrange
            var vehicle = Vehicle.Create(LicensePlate.Create("1234ABC"), Today.AddYears(-1), Today);
            vehicle.Rent();

            // Act
            var act = vehicle.Rent;

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
