using System;

namespace GtMotive.Fleet.Domain.Rentals
{
    /// <summary>
    /// Value object identifying the person who rents a vehicle.
    /// </summary>
    public sealed class RenterId : IEquatable<RenterId>
    {
        private RenterId(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the normalized renter identifier value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a renter identifier from a raw value.
        /// </summary>
        /// <param name="value">Raw renter identifier value.</param>
        /// <returns>A valid <see cref="RenterId"/>.</returns>
        public static RenterId Create(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? throw new DomainException("Renter id cannot be empty.")
                : new RenterId(value.Trim());
        }

        /// <inheritdoc />
        public bool Equals(RenterId other)
        {
            return other is not null && string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as RenterId);

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

        /// <inheritdoc />
        public override string ToString() => Value;
    }
}
