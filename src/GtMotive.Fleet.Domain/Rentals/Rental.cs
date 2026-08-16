using System;

namespace GtMotive.Fleet.Domain.Rentals
{
    /// <summary>
    /// Aggregate root representing the rental of a vehicle by a renter.
    /// </summary>
    public sealed class Rental
    {
        private Rental(Guid id, Guid vehicleId, RenterId renterId, DateOnly startedOn, RentalStatus status)
        {
            Id = id;
            VehicleId = vehicleId;
            RenterId = renterId;
            StartedOn = startedOn;
            Status = status;
        }

        /// <summary>
        /// Gets the unique identifier of the rental.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the identifier of the rented vehicle.
        /// </summary>
        public Guid VehicleId { get; }

        /// <summary>
        /// Gets the identifier of the renter.
        /// </summary>
        public RenterId RenterId { get; }

        /// <summary>
        /// Gets the date on which the rental started.
        /// </summary>
        public DateOnly StartedOn { get; }

        /// <summary>
        /// Gets the current lifecycle state of the rental.
        /// </summary>
        public RentalStatus Status { get; private set; }

        /// <summary>
        /// Gets the date on which the rental was closed, or <c>null</c> while it is active.
        /// </summary>
        public DateOnly? EndedOn { get; private set; }

        /// <summary>
        /// Creates a new active rental.
        /// </summary>
        /// <param name="vehicleId">Identifier of the rented vehicle.</param>
        /// <param name="renterId">Identifier of the renter.</param>
        /// <param name="today">Date on which the rental starts.</param>
        /// <returns>A new active <see cref="Rental"/>.</returns>
        public static Rental Create(Guid vehicleId, RenterId renterId, DateOnly today)
        {
            ArgumentNullException.ThrowIfNull(renterId);

            return new Rental(Guid.NewGuid(), vehicleId, renterId, today, RentalStatus.Active);
        }

        /// <summary>
        /// Closes the rental, enforcing that only an active rental can be closed.
        /// </summary>
        /// <param name="today">Date on which the rental is closed.</param>
        public void Close(DateOnly today)
        {
            if (Status != RentalStatus.Active)
            {
                throw new DomainException("Only an active rental can be closed.");
            }

            Status = RentalStatus.Closed;
            EndedOn = today;
        }
    }
}
