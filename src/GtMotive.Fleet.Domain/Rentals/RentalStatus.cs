namespace GtMotive.Fleet.Domain.Rentals
{
    /// <summary>
    /// Lifecycle state of a rental.
    /// </summary>
    public enum RentalStatus
    {
        /// <summary>
        /// The rental is active: the vehicle is currently rented.
        /// </summary>
        Active,

        /// <summary>
        /// The rental is closed: the vehicle has been returned.
        /// </summary>
        Closed,
    }
}
