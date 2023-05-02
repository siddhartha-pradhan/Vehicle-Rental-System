using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Presentation.Areas.User.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.User.Controllers;

[Area("User")]
public class HomeController : Controller
{
    #region Service Injection
    private readonly IBrandService _brandService;
    private readonly IVehicleService _vehicleService;
    private readonly IOfferService _offerService;

    public HomeController(IBrandService brandService,
        IVehicleService vehicleService,
        IOfferService offerService)
    {
        _brandService = brandService;
        _vehicleService = vehicleService;
        _offerService = offerService;
    }
    #endregion

    #region Razor Views
    [HttpGet]
    public IActionResult Index()
    {
        var brands = _brandService.GetAllBrands()
            .Select(x => new BrandsViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.ImageURL,
            }).ToList();

        var vehicles = (from vehicle in _vehicleService.GetAllVehicles().Where(x => x.IsAvailable)
                        select new VehiclesViewModel
                        {
                            Id = vehicle.Id,
                            Name = $"{vehicle.Model} {_brandService.GetBrand(vehicle.BrandId).Name}",
                            Image = vehicle.Image,
							ImageURL = vehicle.ImageURL,
                            Offer = vehicle.OfferId != null ? $"{_offerService.RetrieveOffer(vehicle.OfferId).Discount}% Offer" : "No offer",
							PricePerDay = $"Rs. {vehicle.PricePerDay}/day"
                        }).DistinctBy(x => x.Id).ToList();

        var details = new HomeViewModel()
        {
            Brand = brands,
            Vehicle = vehicles,
        };

        return View(details);
    }

    [HttpGet]
    public IActionResult Detail(Guid id)
    {
        var vehicle = _vehicleService.GetVehicle(id);

        var result = new GetVehicleViewModel
        {
            Id = id,
            Name = $"{vehicle.Model} - {_brandService.GetBrand(vehicle.BrandId).Name}",
			PlateNumber = vehicle.PlateNumber,
			Image = vehicle.Image,
            ImageURL = vehicle.ImageURL,
            PricePerDay = $"Rs {vehicle.PricePerDay}/day",
            Description = vehicle.Description,
            Features = vehicle.Features,
            Color = vehicle.Color,
        };

        return View(result);

    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    #endregion
}