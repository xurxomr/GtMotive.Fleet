using System.Threading.Tasks;
using GtMotive.Fleet.ApplicationCore.UseCases.ListAvailableVehicles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Fleet.Api.UseCases.ListAvailableVehicles
{
    [ApiController]
    [Route("api/vehicles")]
    public sealed class VehiclesController(IMediator mediator, ListAvailableVehiclesPresenter presenter) : ControllerBase
    {
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            await mediator.Send(new ListAvailableVehiclesInput());

            return presenter.ActionResult;
        }
    }
}
