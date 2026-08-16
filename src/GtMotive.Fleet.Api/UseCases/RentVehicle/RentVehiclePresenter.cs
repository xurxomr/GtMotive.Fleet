using System;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.RentVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Fleet.Api.UseCases.RentVehicle
{
    public sealed class RentVehiclePresenter : IWebApiPresenter, IOutputPortStandard<RentVehicleOutput>
    {
        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(RentVehicleOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var body = new RentVehicleResponse(response.RentalId, response.VehicleId, response.RenterId, response.StartedOn);
            ActionResult = new CreatedResult($"/api/rentals/{response.RentalId}", body);
        }
    }
}
