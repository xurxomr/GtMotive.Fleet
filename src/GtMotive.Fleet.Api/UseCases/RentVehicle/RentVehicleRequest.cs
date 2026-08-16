using System.ComponentModel.DataAnnotations;

namespace GtMotive.Fleet.Api.UseCases.RentVehicle
{
    public sealed class RentVehicleRequest
    {
        [Required]
        public required string RenterId { get; init; }
    }
}
