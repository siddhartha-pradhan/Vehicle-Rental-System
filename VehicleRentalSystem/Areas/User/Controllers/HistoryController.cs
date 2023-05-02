using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Presentation.Areas.User.ViewModels;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Presentation.Areas.User.Controllers;

[Authorize]
[Area("User")]
public class HistoryController : Controller
{
    #region Service Injection
    private readonly IRentalService _rentalService;
    private readonly IAppUserService _appUserService;
    private readonly IVehicleService _vehicleService;
    private readonly IBrandService _brandService;
    private readonly ICustomerService _customerService;
    private readonly IFileTransferService _fileService;
    private readonly IDamageRequestService _damageRequestService;
    private readonly IUnitOfWork _unitOfWork;

    public HistoryController(IRentalService rentalService,
        IAppUserService appUserService,
        IVehicleService vehicleService,
        IBrandService brandService,
        ICustomerService customerService,
        IFileTransferService fileService,
        IDamageRequestService damageRequestService,
		IUnitOfWork unitOfWork)
    {
        _rentalService = rentalService;
        _appUserService = appUserService;
        _vehicleService = vehicleService;
        _brandService = brandService;
        _customerService = customerService;
        _fileService = fileService;
        _damageRequestService = damageRequestService;
        _unitOfWork = unitOfWork;
	}
    #endregion

    #region Razor Views
    [HttpGet]
    public IActionResult Requested()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var vehicles = _vehicleService.GetAllVehicles();
        var brands = _brandService.GetAllBrands();

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
            Image = _vehicleService.GetVehicle(x.VehicleId).Image,
            ImageURL = _vehicleService.GetVehicle(x.VehicleId).ImageURL,
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

        var rentals = _rentalService.GetAllRentals().Where(x => x.UserId == userId && x.RentalStatus == Constants.Approved && x.ReturnedDate == null).ToList();

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
            Image = _vehicleService.GetVehicle(x.VehicleId).Image,
            ImageURL = _vehicleService.GetVehicle(x.VehicleId).ImageURL,
            IsDamaged = x.IsDamaged,
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

        var rentals = _rentalService.GetAllRentals().Where(x => x.UserId == userId && x.RentalStatus == Constants.Approved && x.ReturnedDate != null).ToList();

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
            Image = _vehicleService.GetVehicle(x.VehicleId).Image,
            ImageURL = _vehicleService.GetVehicle(x.VehicleId).ImageURL,
        }).ToList();

        return View(result);
    }

    [HttpGet]
    public IActionResult DamageDetails(Guid rentalId)
    {
        var rental = _rentalService.GetRental(rentalId);

        var vehicle = _vehicleService.GetVehicle(rental.VehicleId);

        var brand = _brandService.GetBrand(vehicle.BrandId);

        var damage = _damageRequestService.GetAllDamageRequests().Where(x => x.RentalId == rental.Id).FirstOrDefault();

        var request = new DamageRequestViewModel()
        {
            VehicleName = $"{vehicle.Model} - {brand.Name}",
            DamageRequest = damage,
        };

        return View(request);
    }

    [HttpGet]
    public IActionResult DamageRequest(Guid rentalId)
    {
        var rental = _rentalService.GetRental(rentalId);

        var vehicle = _vehicleService.GetVehicle(rental.VehicleId);

        var brand = _brandService.GetBrand(vehicle.BrandId);

        var damageRequest = new DamageRequestViewModel()
        {
            DamageRequest = new DamageRequest()
            {
                RentalId = rentalId,
            },
            VehicleName = $"{vehicle.Model} - {brand.Name}"
        };

        return View(damageRequest);
    }
    #endregion

    #region API Calls
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

    [HttpPost]
    public IActionResult DamageRequest(DamageRequestViewModel request, IFormFile image)
    {
        var rental = _rentalService.GetRental(request.DamageRequest.RentalId);

        rental.IsDamaged = true;
        var vehicle = _vehicleService.GetVehicle(rental.VehicleId);
        var brand = _brandService.GetBrand(vehicle.BrandId);
        var user = _appUserService.GetUser(rental.UserId);
        var customer = _customerService.GetUser(user.Id);
        if(customer != null)
        {
            customer.IsActive = false;
        }

        var file = _fileService.FilePath(image, Constants.Damages.ToLower(), $"{user.FullName}'s {vehicle.Model} - {brand.Name}", "");

        var damage = new DamageRequest()
        {
            RentalId = request.DamageRequest.RentalId,
            ImageURL = file,
            DamageDescription = request.DamageRequest.DamageDescription,
        };

        _damageRequestService.AddDamageRequest(damage);

        _unitOfWork.Save();

        TempData["Success"] = "Damage Request successfully notified";
        return RedirectToAction("Accepted");
    }
    #endregion
}
