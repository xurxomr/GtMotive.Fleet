namespace GtMotive.Fleet.Domain.Vehicles
{
    /// <summary>
    /// Availability state of a vehicle within the fleet.
    /// </summary>
    public enum VehicleStatus
    {
        /// <summary>
        /// The vehicle is available to be rented.
        /// </summary>
        Available,

        /// <summary>
        /// The vehicle is currently rented and cannot be rented again.
        /// </summary>
        Rented,
    }
}
