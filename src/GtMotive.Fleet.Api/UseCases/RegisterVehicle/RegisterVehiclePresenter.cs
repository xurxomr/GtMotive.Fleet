using System;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Fleet.Api.UseCases.RegisterVehicle
{
    public sealed class RegisterVehiclePresenter : IWebApiPresenter, IOutputPortStandard<RegisterVehicleOutput>
    {
        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(RegisterVehicleOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var body = new RegisterVehicleResponse(response.Id, response.LicensePlate, response.ManufacturingDate);
            ActionResult = new CreatedResult($"/api/vehicles/{response.Id}", body);
        }
    }
}
