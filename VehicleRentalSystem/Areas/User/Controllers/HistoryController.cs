using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Presentation.Areas.User.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.User.Controllers;

[Area("User")]
[Authorize]

public class HistoryController : Controller
{
    private readonly IRentalService _rentalService;
    private readonly IAppUserService _appUserService;
    private readonly IVehicleService _vehicleService;
    private readonly IBrandService _brandService;
    private readonly IImageService _imageService;
    private readonly ICustomerService _customerService;

    public HistoryController(IRentalService rentalService,
        IAppUserService appUserService,
        IVehicleService vehicleService,
        IImageService imageService,
        IBrandService brandService,
        ICustomerService customerService)
    {
        _rentalService = rentalService;
        _appUserService = appUserService;
        _vehicleService = vehicleService;
        _brandService = brandService;
        _imageService = imageService;
        _customerService = customerService;
    }

    [HttpGet]
    public IActionResult Requested()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var vehicles = _vehicleService.GetAllVehicles();
        var brands = _brandService.GetAllBrands();
        var images = _imageService.GetAllImages();

        var rentals = _rentalService.GetAllRentals().Where(x => x.UserId == userId && x.RentalStatus == Constants.Requested).ToList();

        var result = rentals.Select(x => new HistoryViewModel
        {
            Id = x.Id,
            UserId = x.UserId,
            VehicleId = x.VehicleId,
            VehicleName = $"{_vehicleService.GetVehicle(x.VehicleId).Model} {_brandService.GetBrand(_vehicleService.GetVehicle(x.VehicleId).BrandId).Name}",
            RequestedDate = x.RequestedDate.ToString("dd/MM/yyyy"),
            StartDate = x.StartDate.ToString("dd/MM/yyyy"),
            EndDate = x.EndDate.ToString("dd/MM/yyyy"),
            TotalAmount = $"Rs {x.TotalAmount}",
            VehicleImages = _imageService.GetVehicleImages(x.VehicleId),
        }).ToList();

        return View(result);
    }

    [HttpGet]
    public IActionResult Accepted()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var vehicles = _vehicleService.GetAllVehicles();
        var brands = _brandService.GetAllBrands();
        var images = _imageService.GetAllImages();

        var rentals = _rentalService.GetAllRentals().Where(x => x.UserId == userId && x.RentalStatus == Constants.Approved).ToList();

        var result = rentals.Select(x => new HistoryViewModel
        {
            Id = x.Id,
            UserId = x.UserId,
            VehicleId = x.VehicleId,
            VehicleName = $"{_vehicleService.GetVehicle(x.VehicleId).Model} {_brandService.GetBrand(_vehicleService.GetVehicle(x.VehicleId).BrandId).Name}",
            RequestedDate = x.RequestedDate.ToString("dd/MM/yyyy"),
            StartDate = x.StartDate.ToString("dd/MM/yyyy"),
            EndDate = x.EndDate.ToString("dd/MM/yyyy"),
            TotalAmount = $"Rs {x.TotalAmount}",
            ActionBy = _appUserService.GetUser(x.ActionBy).FullName,
            ActionDate = x.ActionDate?.ToString("dd/MM/yyyy"),
            VehicleImages = _imageService.GetVehicleImages(x.VehicleId),
        }).ToList();

        return View(result);
    }

    [HttpGet]
    public IActionResult Returned()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var vehicles = _vehicleService.GetAllVehicles();
        var brands = _brandService.GetAllBrands();
        var images = _imageService.GetAllImages();

        var rentals = _rentalService.GetAllRentals().Where(x => x.UserId == userId && x.RentalStatus == Constants.Returned).ToList();

        var result = rentals.Select(x => new HistoryViewModel
        {
            Id = x.Id,
            UserId = x.UserId,
            VehicleId = x.VehicleId,
            VehicleName = $"{_vehicleService.GetVehicle(x.VehicleId).Model} {_brandService.GetBrand(_vehicleService.GetVehicle(x.VehicleId).BrandId).Name}",
            RequestedDate = x.RequestedDate.ToString("dd/MM/yyyy"),
            StartDate = x.StartDate.ToString("dd/MM/yyyy"),
            EndDate = x.EndDate.ToString("dd/MM/yyyy"),
            ReturnedDate = x.ReturnedDate?.ToString("dd/MM/yyyy"),
            TotalAmount = $"Rs {x.TotalAmount}",
            ActionBy = _appUserService.GetUser(x.ActionBy).FullName,
            ActionDate = x.ActionDate?.ToString("dd/MM/yyyy"),
            VehicleImages = _imageService.GetVehicleImages(x.VehicleId),
        }).ToList();

        return View(result);
    }

    [HttpGet]
    public IActionResult Rejected()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var vehicles = _vehicleService.GetAllVehicles();
        var brands = _brandService.GetAllBrands();
        var images = _imageService.GetAllImages();

        var rentals = _rentalService.GetAllRentals().Where(x => x.UserId == userId && x.RentalStatus == Constants.Rejected).ToList();

        var result = rentals.Select(x => new HistoryViewModel
        {
            Id = x.Id,
            UserId = x.UserId,
            VehicleId = x.VehicleId,
            VehicleName = $"{_vehicleService.GetVehicle(x.VehicleId).Model} {_brandService.GetBrand(_vehicleService.GetVehicle(x.VehicleId).BrandId).Name}",
            RequestedDate = x.RequestedDate.ToString("dd/MM/yyyy"),
            StartDate = x.StartDate.ToString("dd/MM/yyyy"),
            EndDate = x.EndDate.ToString("dd/MM/yyyy"),
            TotalAmount = $"Rs {x.TotalAmount}",
            ActionBy = _appUserService.GetUser(x.ActionBy).FullName,
            ActionDate = x.ActionDate?.ToString("dd/MM/yyyy"),
            VehicleImages = _imageService.GetVehicleImages(x.VehicleId),
        }).ToList();

        return View(result);
    }

    [HttpPost]
    public IActionResult Requested(Guid rentalId)
    {
        _rentalService.CancelRent(rentalId);
        TempData["Success"] = "Rent successfully canceled";
        return RedirectToAction("Requested");
    }

	[HttpPost]
	public IActionResult Accepted(Guid rentalId)
	{
		_rentalService.CancelRent(rentalId);
		TempData["Success"] = "Rent successfully canceled";
		return RedirectToAction("Accepted");
	}
}
