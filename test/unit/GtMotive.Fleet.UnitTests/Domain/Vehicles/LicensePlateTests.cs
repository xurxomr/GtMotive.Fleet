using FluentAssertions;
using GtMotive.Fleet.Domain;
using GtMotive.Fleet.Domain.Vehicles;
using Xunit;

namespace GtMotive.Fleet.UnitTests.Domain.Vehicles
{
    public sealed class LicensePlateTests
    {
        [Fact]
        public void Create_ValueWithSurroundingWhitespaceAndLowerCase_NormalizesToTrimmedUpperCase()
        {
            // Arrange
            const string rawValue = "  1234abc  ";

            // Act
            var licensePlate = LicensePlate.Create(rawValue);

            // Assert
            licensePlate.Value.Should().Be("1234ABC");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_EmptyValue_ThrowsDomainException(string value)
        {
            // Arrange & Act
            var act = () => LicensePlate.Create(value);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Equals_SameNormalizedValue_ReturnsTrue()
        {
            // Arrange
            var first = LicensePlate.Create("1234abc");
            var second = LicensePlate.Create("1234ABC");

            // Act
            var areEqual = first.Equals(second);

            // Assert
            areEqual.Should().BeTrue();
        }
    }
}
