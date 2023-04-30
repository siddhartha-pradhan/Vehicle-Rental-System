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
            ActualPrice = vehicle.PricePerDay,
        };

        if(role == Constants.Admin || role == Constants.Staff)
        {
            rent.PriceForRegularAndStaffs = vehicle.PricePerDay - (25/100 * vehicle.PricePerDay);
        } 
        else if(role == Constants.Customer) 
        {
            var customer = _customerService.GetUser(user.Id);

            rent.CustomerCitizenshipNumber = customer.CitizenshipNumber;
            rent.CustomerLicenseNumber = customer.CitizenshipNumber;
        }

        return View(rent);
    }
}
