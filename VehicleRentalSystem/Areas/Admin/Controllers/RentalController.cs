using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;
using System.Security.Claims;
using VehicleRentalSystem.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using VehicleRentalSystem.Domain.Entities;
using System.Xml.Linq;
using VehicleRentalSystem.Infrastructure.Implementation.Repositories;

namespace VehicleRentalSystem.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{Constants.Admin}, {Constants.Staff}")]
public class RentalController : Controller
{
	private readonly IRentalService _rentalService;
	private readonly IAppUserService _appUserService;
	private readonly IVehicleService _vehicleService;
	private readonly IBrandService _brandService;
	private readonly IImageService _imageService;
	private readonly ICustomerService _customerService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IEmailSender _emailSender;
	public RentalController(IRentalService rentalService, 
		IAppUserService appUserService, 
		IVehicleService vehicleService, 
		IBrandService brandService, 
		IImageService imageService, 
		ICustomerService customerService,
		IUnitOfWork unitOfWork,
		IEmailSender emailSender)
	{
		_rentalService = rentalService;
		_appUserService = appUserService;
		_vehicleService = vehicleService;
		_brandService = brandService;
		_imageService = imageService;
		_unitOfWork = unitOfWork;
		_customerService = customerService;
		_emailSender = emailSender;
	}

	[HttpGet]
	public IActionResult Requested()
	{
		var vehicles = _vehicleService.GetAllVehicles();
		var brands = _brandService.GetAllBrands();
		var images = _imageService.GetAllImages();

		var rentals = _rentalService.GetAllRentals().Where(x => x.RentalStatus == Constants.Requested).ToList();

		var result = rentals.Select(x => new RentalDetailsViewModel
		{
			Id = x.Id,
			UserId = x.UserId,
			UserName = _appUserService.GetUser(x.UserId).FullName,
			PhoneNumber = _appUserService.GetUser(x.UserId).PhoneNumber,
			CitizenshipNumber = _customerService.GetUser(x.UserId) != null ? _customerService.GetUser(x.UserId).CitizenshipNumber : "Not a customer",
			CitizenshipURL = _customerService.GetUser(x.UserId) != null ? _customerService.GetUser(x.UserId).CitizenshipURL : "Not a customer",
			LicenseNumber = _customerService.GetUser(x.UserId) != null ? _customerService.GetUser(x.UserId).LicenseNumber : "Not a customer",
			LicenseURL = _customerService.GetUser(x.UserId) != null ? _customerService.GetUser(x.UserId).LicenseURL : "Not a customer",
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
		var vehicles = _vehicleService.GetAllVehicles();
		var brands = _brandService.GetAllBrands();
		var images = _imageService.GetAllImages();

		var rentals = _rentalService.GetAllRentals().Where(x => x.RentalStatus == Constants.Approved).ToList();

		var result = rentals.Select(x => new RentalDetailsViewModel
		{
			Id = x.Id,
			UserId = x.UserId,
			VehicleId = x.VehicleId,
			UserName = _appUserService.GetUser(x.UserId).FullName,
			PhoneNumber = _appUserService.GetUser(x.UserId).PhoneNumber,
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
		var vehicles = _vehicleService.GetAllVehicles();
		var brands = _brandService.GetAllBrands();
		var images = _imageService.GetAllImages();

		var rentals = _rentalService.GetAllRentals().Where(x => x.RentalStatus == Constants.Returned).ToList();

		var result = rentals.Select(x => new RentalDetailsViewModel
		{
			Id = x.Id,
			UserId = x.UserId,
			VehicleId = x.VehicleId,
			UserName = _appUserService.GetUser(x.UserId).FullName,
			PhoneNumber = _appUserService.GetUser(x.UserId).PhoneNumber,
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

	[HttpPost]
	public IActionResult AcceptRent(Guid rentalId)
	{
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

		var rent = _rentalService.GetRental(rentalId);

		var user = _appUserService.GetUser(rent.UserId);

		var vehicle = $"{_vehicleService.GetVehicle(rent.VehicleId).Model} {_brandService.GetBrand(_vehicleService.GetVehicle(rent.VehicleId).BrandId).Name}"; 

		rent.IsApproved = true;
		rent.RentalStatus = Constants.Approved;
		rent.ActionDate = DateTime.Now;
		rent.ActionBy = claim.Value;

		_unitOfWork.Save();

		TempData["Success"] = "Rent accepted successfully";

		_emailSender.SendEmailAsync(user.Email, "Successful Rent",
					$"Dear {user.FullName},<br><br>Your rent for the vehicle {vehicle} has been approved. " +
					$"<br>Kindly check your system for the details and visit the store to recieve the vehicle." +
					$"<br><br>Regards,<br>Hajur ko Car Rental");

		return RedirectToAction("Requested");
	}

	[HttpPost]
	public IActionResult RejectRent(Guid rentalId)
	{
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

		var rent = _rentalService.GetRental(rentalId);

		var user = _appUserService.GetUser(rent.UserId);

		var vehicle = $"{_vehicleService.GetVehicle(rent.VehicleId).Model} {_brandService.GetBrand(_vehicleService.GetVehicle(rent.VehicleId).BrandId).Name}";

		_rentalService.CancelRent(rentalId);

		TempData["Success"] = "Rent rejected successfully";

		_emailSender.SendEmailAsync(user.Email, "Unsuccessful Rent",
					$"Dear {user.FullName},<br><br>Your rent for the vehicle {vehicle} has been rejected. " +
					$"<br>Kindly visit the store to know about the rejection." +
					$"<br><br>Regards,<br>Hajur ko Car Rental");

		return RedirectToAction("Requested");

	}
}
