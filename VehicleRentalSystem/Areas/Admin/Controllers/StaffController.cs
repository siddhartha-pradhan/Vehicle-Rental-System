using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Constants;

namespace VehicleRentalSystem.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.Admin)]
public class StaffController : Controller
{
    #region Razor Views
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Upsert(Guid? id)
    {
        return View();
    }

    public IActionResult Delete(Guid id)
    {
        return View();
    }
    #endregion

    #region API Calls

    #endregion
}
