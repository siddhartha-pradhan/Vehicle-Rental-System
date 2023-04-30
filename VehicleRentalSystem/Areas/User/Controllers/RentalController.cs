using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Presentation.Areas.User.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.User.Controllers;

[Authorize]
[Area("User")]
public class RentalController : Controller
{
    private readonly IAppUserService _appUserService;
    private readonly ICustomerService _customerService;
    private readonly IVehicleService _vehicleService;
    private readonly IRentalService _rentalService;
    private readonly IImageService _imageService;
    private readonly IBrandService _brandService;
    private readonly UserManager<IdentityUser> _userManager;

    public RentalController(IAppUserService appUserService, 
        ICustomerService customerService, 
        IVehicleService vehicleService, 
        IRentalService rentalService, 
        IImageService imageService, 
        IBrandService brandService,
        UserManager<IdentityUser> userManager)
    {
        _appUserService = appUserService;
        _customerService = customerService;
        _vehicleService = vehicleService;
        _rentalService = rentalService;
        _imageService = imageService;
        _brandService = brandService;
        _userManager = userManager;
    }

    public IActionResult Rental(Guid vehicleId)
    {
        var vehicle = _vehicleService.GetVehicle(vehicleId);
        var brand = _brandService.GetBrand(vehicle.BrandId);
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var user = _appUserService.GetUser(userId);
        var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

        var rent = new RentalViewModel()
        {
            VehicleId = vehicleId,
            VehicleName = $"{vehicle.Model} - {brand.Name}",
            VehicleDescription = vehicle.Description,
            VehicleFeatures = vehicle.Features,
            CustomerId = userId,
            CustomerName = user.FullName,
            CustomerAddress = user.Address,
            CustomerState = user.State,
            PhoneNumber = user.PhoneNumber,
            ActualPrice = vehicle.PricePerDay,
        };

        if(role == Constants.Admin || role == Constants.Staff)
        {
            rent.PriceForRegularAndStaffs = vehicle.PricePerDay - (0.25 * vehicle.PricePerDay);
        } 
        else if(role == Constants.Customer) 
        {
            var customer = _customerService.GetUser(user.Id);

            rent.CustomerCitizenshipNumber = customer.CitizenshipNumber == null ? "No citizenship found" : customer.CitizenshipNumber;
            rent.CustomerLicenseNumber = customer.LicenseNumber == null ? "No license found" : customer.LicenseNumber;
        }

        return View(rent);
    }

    [HttpPost]
    public IActionResult Rental(RentalViewModel model)
    {
        var vehicle = _vehicleService.GetVehicle(model.VehicleId);
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var user = _appUserService.GetUser(userId);
        var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
        var price = 0.0;
        var days = ((model.EndDate - model.StartDate).TotalDays);

        if (role == Constants.Admin || role == Constants.Staff)
        {
            price = vehicle.PricePerDay - (0.25 * vehicle.PricePerDay);
        }
        else if (role == Constants.Customer)
        {
            var customer = _customerService.GetUser(user.Id);
            
            price = vehicle.PricePerDay;
            
            if(customer.LicenseURL == null || customer.CitizenshipURL == null)
            {
                TempData["Delete"] = "Please add your citizenship and license before renting a car";

                return RedirectToAction("Documents", "Profile", new { area = "Account" });
            }
        }

        var result = new Rental()
        {
            UserId = model.CustomerId,
            VehicleId = model.VehicleId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            TotalAmount = (float)(days * price),
        };

        vehicle.IsAvailable = false;
        _vehicleService.UpdateVehicle(vehicle);

        _rentalService.AddRental(result);
        TempData["Success"] = "Your rental request has been notified.";
        return RedirectToAction("Index", "Home");
    }
}
