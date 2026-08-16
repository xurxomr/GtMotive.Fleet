using System;

namespace GtMotive.Fleet.Domain.Vehicles
{
    /// <summary>
    /// Value object representing a vehicle license plate.
    /// </summary>
    public sealed class LicensePlate : IEquatable<LicensePlate>
    {
        private LicensePlate(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the normalized license plate value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a license plate from a raw value, normalizing it to trimmed upper case.
        /// </summary>
        /// <param name="value">Raw license plate value.</param>
        /// <returns>A valid <see cref="LicensePlate"/>.</returns>
        public static LicensePlate Create(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? throw new DomainException("License plate cannot be empty.")
                : new LicensePlate(value.Trim().ToUpperInvariant());
        }

        /// <inheritdoc />
        public bool Equals(LicensePlate other)
        {
            return other is not null && string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as LicensePlate);

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

        /// <inheritdoc />
        public override string ToString() => Value;
    }
}
