using System;

namespace GtMotive.Fleet.Domain.Vehicles
{
    /// <summary>
    /// Aggregate root representing a vehicle in the renting fleet.
    /// </summary>
    public sealed class Vehicle
    {
        /// <summary>
        /// Maximum age, in years, a vehicle may have based on its manufacturing date to be admitted into the fleet.
        /// </summary>
        public const int MaxManufacturingAgeInYears = 5;

        private Vehicle(Guid id, LicensePlate licensePlate, DateOnly manufacturingDate, VehicleStatus status)
        {
            Id = id;
            LicensePlate = licensePlate;
            ManufacturingDate = manufacturingDate;
            Status = status;
        }

        /// <summary>
        /// Gets the unique identifier of the vehicle.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the license plate of the vehicle.
        /// </summary>
        public LicensePlate LicensePlate { get; }

        /// <summary>
        /// Gets the manufacturing date of the vehicle.
        /// </summary>
        public DateOnly ManufacturingDate { get; }

        /// <summary>
        /// Gets the current availability status of the vehicle.
        /// </summary>
        public VehicleStatus Status { get; private set; }

        /// <summary>
        /// Creates a new available vehicle, enforcing the fleet age invariant.
        /// </summary>
        /// <param name="licensePlate">License plate of the vehicle.</param>
        /// <param name="manufacturingDate">Manufacturing date of the vehicle.</param>
        /// <param name="today">Reference date used to evaluate the vehicle age.</param>
        /// <returns>A new available <see cref="Vehicle"/>.</returns>
        public static Vehicle Create(LicensePlate licensePlate, DateOnly manufacturingDate, DateOnly today)
        {
            ArgumentNullException.ThrowIfNull(licensePlate);

            var oldestAllowedManufacturingDate = today.AddYears(-MaxManufacturingAgeInYears);

            return manufacturingDate < oldestAllowedManufacturingDate
                ? throw new DomainException($"A vehicle manufacturing date cannot be older than {MaxManufacturingAgeInYears} years.")
                : new Vehicle(Guid.NewGuid(), licensePlate, manufacturingDate, VehicleStatus.Available);
        }

        /// <summary>
        /// Marks the vehicle as rented, enforcing that only available vehicles can be rented.
        /// </summary>
        public void Rent()
        {
            if (Status != VehicleStatus.Available)
            {
                throw new DomainException("Only an available vehicle can be rented.");
            }

            Status = VehicleStatus.Rented;
        }

        /// <summary>
        /// Marks the vehicle as available again, enforcing that only rented vehicles can be returned.
        /// </summary>
        public void Return()
        {
            if (Status != VehicleStatus.Rented)
            {
                throw new DomainException("Only a rented vehicle can be returned.");
            }

            Status = VehicleStatus.Available;
        }
    }
}
