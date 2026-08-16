using System;
using System.Threading.Tasks;
using GtMotive.Fleet.ApplicationCore.UseCases.ReturnVehicle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Fleet.Api.UseCases.ReturnVehicle
{
    [ApiController]
    [Route("api/vehicles")]
    public sealed class VehiclesController(IMediator mediator, ReturnVehiclePresenter presenter) : ControllerBase
    {
        [HttpPost("{vehicleId:guid}/return")]
        public async Task<IActionResult> Return(Guid vehicleId)
        {
            await mediator.Send(new ReturnVehicleInput(vehicleId));

            return presenter.ActionResult;
        }
    }
}
