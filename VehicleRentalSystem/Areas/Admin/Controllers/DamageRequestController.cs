using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Constants;
using Microsoft.AspNetCore.Identity.UI.Services;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{Constants.Admin}, {Constants.Staff}")]
public class DamageRequestController : Controller
{
    #region Service Injection
    private readonly IDamageRequestService _damageRequestService;
    private readonly IVehicleService _vehicleService;
    private readonly ICustomerService _customerService;
    private readonly IBrandService _brandService;
    private readonly IRentalService _rentalService;
    private readonly IAppUserService _appUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;

    public DamageRequestController(IDamageRequestService damageRequestService,
        IVehicleService vehicleService,
        ICustomerService customerService,
        IBrandService brandService,
        IRentalService rentalService,
        IAppUserService appUserService,
        IUnitOfWork unitOfWork,
        IEmailSender emailSender)
    {
        _damageRequestService = damageRequestService;
        _vehicleService = vehicleService;
        _customerService = customerService;
        _brandService = brandService;
        _rentalService = rentalService;
        _appUserService = appUserService;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }
    #endregion

    #region Razor Views
    [HttpGet]
    public IActionResult Index()
    {
        var rentals = _rentalService.GetAllRentals();
        var requests = _damageRequestService.GetAllDamageRequests();
        var vehicles = _vehicleService.GetAllVehicles();
        var brand = _brandService.GetAllBrands();
        var users = _appUserService.GetAllUsers();

        var result = (from rental in rentals
                      join request in requests
                         on rental.Id equals request.RentalId
                      join vehicle in vehicles
                         on rental.VehicleId equals vehicle.Id
                      join user in users
                         on rental.UserId equals user.Id
                      join staff in users
                         on rental.ActionBy equals staff.Id
                      select new DamageViewModel()
                      {
                          DamageId = request.Id,
                          Description = request.DamageDescription,
                          RequestDate = request.DamageRequestDate.ToString("dd/MM/yyyy"),
                          ApprovedDate = rental.ActionDate?.ToString("dd/MM/yyyy"),
                          RentalId = rental.Id,
                          VehicleId = rental.VehicleId,
                          VehicleName = $"{vehicle.Model} {_brandService.GetBrand(vehicle.BrandId).Name}",
                          CustomerId = user.Id,
                          CustomerName = user.FullName,
                          CustomerPhone = user.PhoneNumber,
                          ApproverId = staff.Id,
                          ApproverName = staff.FullName,
                          PaymentStatus = request.IsPaid ? "Paid" : "Unpaid",
                          Cost = request.RepairCost == null ? "Not charged yet" : $"Rs {request.RepairCost}",
                          DamageImage = request.ImageURL
                      }).ToList();
        return View(result);
    }

    [HttpGet]
    public IActionResult Details(Guid id)
    {
        var rentals = _rentalService.GetAllRentals();
        var requests = _damageRequestService.GetAllDamageRequests().Where(x => x.Id == id);
        var vehicles = _vehicleService.GetAllVehicles();
        var brand = _brandService.GetAllBrands();
        var users = _appUserService.GetAllUsers();

        var result = (from rental in rentals
                      join request in requests
                         on rental.Id equals request.RentalId
                      join vehicle in vehicles
                         on rental.VehicleId equals vehicle.Id
                      join user in users
                         on rental.UserId equals user.Id
                      join staff in users
                         on rental.ActionBy equals staff.Id
                      select new DamageViewModel()
                      {
                          DamageId = request.Id,
                          Description = request.DamageDescription,
                          RequestDate = request.DamageRequestDate.ToString("dd/MM/yyyy"),
                          ApprovedDate = rental.ActionDate?.ToString("dd/MM/yyyy"),
                          RentalId = rental.Id,
                          VehicleId = rental.VehicleId,
                          VehicleName = $"{vehicle.Model} {_brandService.GetBrand(vehicle.BrandId).Name}",
                          CustomerId = user.Id,
                          CustomerName = user.FullName,
                          CustomerPhone = user.PhoneNumber,
                          ApproverId = staff.Id,
                          ApproverName = staff.FullName,
                          PaymentStatus = request.IsPaid ? "Paid" : "Unpaid",
                          Cost = request.RepairCost == null ? "Not charged yet" : $"Rs {request.RepairCost}",
                          DamageImage = request.ImageURL
                      }).FirstOrDefault();

        return View(result);
    }
    #endregion

    #region API Calls
    [HttpPost]
    public IActionResult Details(DamageViewModel damage)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        var request = _damageRequestService.DamageRequest(damage.DamageId);
        var rental = _rentalService.GetRental(request.RentalId);
        var user = _appUserService.GetUser(rental.UserId);
        var vehicle = _vehicleService.GetVehicle(rental.VehicleId);
        var brand = _brandService.GetBrand(vehicle.BrandId);
        request.RepairCost = damage.RepairCost;
        request.IsPaid = damage.PaymentStatus == "true" ? true : false;
        request.IsApproved = true;
        request.ActionDate = DateTime.Now;
        request.ApprovedBy = claim.Value;
        _unitOfWork.Save();

        if (request.IsPaid == true)
        {
            rental.IsReturned = true;

            rental.ReturnedDate = DateTime.Now;

            var customer = _customerService.GetUser(rental.UserId);
            
            if (customer != null)
            {
                customer.IsActive = true;
            }

            _unitOfWork.Save();

            _emailSender.SendEmailAsync(user.Email, "Successful Payment",
                $"Dear {user.FullName},<br><br>Your request for the damage on vehicle {vehicle.Model} - {brand.Name} has been accepted. " +
                    $"<br>You can now surf the store and rent other vehicles." +
                    $"<br><br>Regards,<br>Hajur ko Car Rental");

            TempData["Success"] = "Payment verified successfully ";

            return RedirectToAction("Index");
        }

        _emailSender.SendEmailAsync(user.Email, "Damage Request Update",
                $"Dear {user.FullName},<br><br>Your request for the damage on vehicle {vehicle.Model} - {brand.Name} has been validated. " +
                    $"<br>Please check the system to examine updated details." +
                    $"<br><br>Regards,<br>Hajur ko Car Rental");

        TempData["Success"] = "Request successfully updated";

        return RedirectToAction("Index");

    }
    #endregion
}
