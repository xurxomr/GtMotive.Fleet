using FluentAssertions;
using GtMotive.Fleet.Domain;
using GtMotive.Fleet.Domain.Rentals;
using Xunit;

namespace GtMotive.Fleet.UnitTests.Domain.Rentals
{
    public sealed class RenterIdTests
    {
        [Fact]
        public void Create_ValueWithSurroundingWhitespace_TrimsValue()
        {
            // Arrange
            const string rawValue = "  renter-1  ";

            // Act
            var renterId = RenterId.Create(rawValue);

            // Assert
            renterId.Value.Should().Be("renter-1");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_EmptyValue_ThrowsDomainException(string value)
        {
            // Arrange & Act
            var act = () => RenterId.Create(value);

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
