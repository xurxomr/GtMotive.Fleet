using System;
using System.Threading.Tasks;
using GtMotive.Fleet.ApplicationCore.UseCases.RegisterVehicle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Fleet.Api.UseCases.RegisterVehicle
{
    [ApiController]
    [Route("api/vehicles")]
    public sealed class VehiclesController(
        IMediator mediator,
        RegisterVehiclePresenter presenter)
        : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await mediator.Send(new RegisterVehicleInput(request.LicensePlate, request.ManufacturingDate));

            return presenter.ActionResult;
        }
    }
}
