using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.Admin)]
public class VehicleController : Controller
{
    #region Service Injection
    private readonly IBrandService _brandService;
    private readonly IAppUserService _appUserService;
    private readonly IFileTransferService _fileService;
    private readonly IOfferService _offerService;
    private readonly IVehicleService _vehicleService;
    private readonly IRentalService _rentalService;

    public VehicleController(IBrandService brandService, 
        IAppUserService appUserService, 
        IFileTransferService fileService, 
        IOfferService offerService, 
        IVehicleService vehicleService,
        IRentalService rentalService)
    {
        _brandService = brandService;
        _appUserService = appUserService;
        _fileService = fileService;
        _offerService = offerService;
        _vehicleService = vehicleService;
        _rentalService = rentalService;
    }
    #endregion

    #region Razor Views
    [HttpGet]
    public IActionResult Index()
    {
        var brands = _brandService.GetAllBrands();
        var vehicles = _vehicleService.GetAllVehicles();
        var offers = _offerService.GetAllOffers();

        var result = (from brand in brands
                      join vehicle in vehicles
                         on brand.Id equals vehicle.BrandId
                      join offer in offers
                          on vehicle.OfferId equals offer.Id into offerGroup
                      from offer in offerGroup.DefaultIfEmpty()
                      select new VehicleViewModel
                      {
                          Id = vehicle.Id,
                          Image = vehicle.Image,
                          ImageURL = vehicle.ImageURL,
                          Brand = brand.Name,
                          Color = vehicle.Color,
                          Model = vehicle.Model,
                          PlateNumber = vehicle.PlateNumber,
                          PricePerDay = $"Rs {vehicle.PricePerDay} /-",
                          TotalRents = _rentalService.GetAllRentals().Where(x => x.VehicleId == vehicle.Id).Count(),
                          Availablility = vehicle.IsAvailable ? "Yes" : "No",
                          Offer = offer != null ? "Yes" : "No"
                      }).ToList();

        return View(result);
    }

    [HttpGet]
    public IActionResult Details(Guid id)
    {
        var brands = _brandService.GetAllBrands();
        var vehicles = _vehicleService.GetAllVehicles().Where(x => x.Id == id).ToList();
        var offers = _offerService.GetAllOffers();
        var users = _appUserService.GetAllUsers();

        var results = (from brand in brands
                       join vehicle in vehicles
                          on brand.Id equals vehicle.BrandId
                       join user in users
                          on vehicle.CreatedBy equals user.Id
                       join offer in offers
                           on vehicle.OfferId equals offer.Id into offerGroup
                       from offer in offerGroup.DefaultIfEmpty()
                       select new VehicleViewModel()
                       {
                           Id = vehicle.Id,
                           Brand = brand.Name,
                           Color = vehicle.Color,
                           Model = vehicle.Model,
                           Description = vehicle.Description,
                           Features = vehicle.Features,
                           PlateNumber = vehicle.PlateNumber,
                           PricePerDay = $"Rs {vehicle.PricePerDay} /-",
                           Availablility = vehicle.IsAvailable ? "Yes" : "No",
                           Offer = offer != null ? "Yes" : "No",
                           CreatedBy = user.FullName,
                           CreatedDate = vehicle.CreatedDate.ToString("dd/MM/yyyy"),
                           Image = vehicle.Image,
                           ImageURL = vehicle.ImageURL,
                       }).FirstOrDefault();

        var rents = (from rent in _rentalService.GetAllRentals().Where(x => x.RentalStatus == Constants.Approved && x.VehicleId == id)
                     join user in _appUserService.GetAllUsers()
                        on rent.UserId equals user.Id
                     join staff in _appUserService.GetAllUsers()
                        on rent.ActionBy equals staff.Id
                     select new VehicleDetailRentViewModel()
                     {
                         CustomerName = user.FullName,
                         RentedDays = (rent.EndDate - rent.StartDate).TotalDays,
                         ReturnedDate = rent.ReturnedDate != null ? rent.ReturnedDate?.ToString("dd/MM/yyyy") : "Not returned yet",
                         ApprovedStaff = staff.FullName,
                         TotalAmount = $"Rs {_rentalService.GetRental(rent.Id).TotalAmount}"
                     }).ToList();

        var detail = new VehicleDetailViewModel()
        {
            VehicleImageViewModel = results,
            RentailDetails = rents
        };

        return View(detail);
    }

    [HttpGet]
    public IActionResult Upsert(Guid id)
    {
        var vehicle = new Vehicle();

        if(id == Guid.Empty)
        {
            ViewBag.BrandId = new SelectList(_brandService.GetAllBrands(), "Id", "Name");

            return View(vehicle);
        }

        vehicle = _vehicleService.GetVehicle(id);

        if(vehicle == null)
        {
            return NotFound();
        }

        ViewBag.BrandId = new SelectList(_brandService.GetAllBrands(), "Id", "Name", vehicle.BrandId);

        return View(vehicle);
    }

    [HttpGet]
    public IActionResult Delete(Guid id)
    {
        var vehicle = _vehicleService.GetVehicle(id);

        if (vehicle == null)
        {
            return NotFound();
        }

        ViewBag.BrandId = new SelectList(_brandService.GetAllBrands(), "Id", "Name", vehicle.BrandId);

        return View(vehicle);
    }
    #endregion

    #region API Calls
    [HttpPost, ActionName("Upsert")]
    public IActionResult UpsertVehicle(Vehicle vehicle)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;

        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        var image = Request.Form.Files.FirstOrDefault();

        if(vehicle.Id == Guid.Empty)
        {
            var vehicleId = Guid.NewGuid();

            var item = new Vehicle()
            {
                Id = vehicleId,
                Model = vehicle.Model,
                Color = vehicle.Color,
                PlateNumber = vehicle.PlateNumber,
                PricePerDay = vehicle.PricePerDay,
                Description = vehicle.Description.Replace("<p>", "").Replace("</p>", ""),
                Features = vehicle.Features.Replace("<p>", "").Replace("</p>", ""),
                BrandId = vehicle.BrandId,
                CreatedBy = claim.Value,
                Image = _fileService.ImageByte(image),
                ImageURL = _fileService.FilePath(image, Constants.Vehicle, $"{vehicle.Model}", "")
            };

            _vehicleService.AddVehicle(item);

            TempData["Success"] = "Vehicle successfully added";
        }
        else
        {
            var item = new Vehicle()
            {
                Id = vehicle.Id,
                Model = vehicle.Model,
                Color = vehicle.Color,
                PlateNumber = vehicle.PlateNumber,
                PricePerDay = vehicle.PricePerDay,
                Description = vehicle.Description.Replace("<p>", "").Replace("</p>", ""),
                Features = vehicle.Features.Replace("<p>", "").Replace("</p>", ""),
                BrandId = vehicle.BrandId,
                LastModifiedBy = claim.Value,
                IsAvailable = vehicle.IsAvailable,
            };

            _vehicleService.UpdateVehicle(item);

            TempData["Success"] = "Vehicle successfully updated";

        }

        return RedirectToAction("Index");
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteVehicle(Guid id)
    {
        var vehicle = _vehicleService.GetVehicle(id);

        if (vehicle != null)
        {
            _vehicleService.DeleteVehicle(vehicle.Id);

            TempData["Delete"] = "Vehicle successfully deleted";

            return RedirectToAction("Index");
        }

        return NotFound();
    }

    #endregion
}
