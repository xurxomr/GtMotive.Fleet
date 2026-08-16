using System;
using System.Threading.Tasks;
using GtMotive.Fleet.ApplicationCore.UseCases.RentVehicle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Fleet.Api.UseCases.RentVehicle
{
    [ApiController]
    [Route("api/vehicles")]
    public sealed class VehiclesController(IMediator mediator, RentVehiclePresenter presenter) : ControllerBase
    {
        [HttpPost("{vehicleId:guid}/rentals")]
        public async Task<IActionResult> Rent(Guid vehicleId, [FromBody] RentVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await mediator.Send(new RentVehicleInput(vehicleId, request.RenterId));

            return presenter.ActionResult;
        }
    }
}
