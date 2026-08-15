using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Fleet.Api.UseCases
{
    public interface IWebApiPresenter
    {
        IActionResult ActionResult { get; }
    }
}
