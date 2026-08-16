using System;
using System.Linq;
using GtMotive.Fleet.ApplicationCore.UseCases;
using GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Fleet.Api.UseCases.ListAvailableVehicles
{
    public sealed class ListAvailableVehiclesPresenter : IWebApiPresenter, IOutputPortStandard<ListAvailableVehiclesOutput>
    {
        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(ListAvailableVehiclesOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var vehicles = response.Vehicles
                .Select(vehicle => new AvailableVehicleResponse(vehicle.Id, vehicle.LicensePlate, vehicle.ManufacturingDate))
                .ToList();

            ActionResult = new OkObjectResult(new ListAvailableVehiclesResponse(vehicles));
        }
    }
}
