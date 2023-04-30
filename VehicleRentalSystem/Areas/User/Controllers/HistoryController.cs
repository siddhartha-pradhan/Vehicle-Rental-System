using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VehicleRentalSystem.Presentation.Areas.User.Controllers;

[Area("User")]
[Authorize]

public class HistoryController : Controller
{

    public IActionResult Index()
    {
        return View();
    }
}
