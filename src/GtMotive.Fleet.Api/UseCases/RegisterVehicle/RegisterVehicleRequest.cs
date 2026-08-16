using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Fleet.Api.UseCases.RegisterVehicle
{
    public sealed class RegisterVehicleRequest
    {
        [Required]
        public required string LicensePlate { get; init; }

        [Required]
        public required DateOnly ManufacturingDate { get; init; }
    }
}
