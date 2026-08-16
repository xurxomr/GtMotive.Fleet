using System;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.ReturnVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Fleet.Api.UseCases.ReturnVehicle
{
    public sealed class ReturnVehiclePresenter : IWebApiPresenter, IOutputPortStandard<ReturnVehicleOutput>
    {
        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(ReturnVehicleOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var body = new ReturnVehicleResponse(response.VehicleId, response.RentalId, response.EndedOn);
            ActionResult = new OkObjectResult(body);
        }
    }
}
