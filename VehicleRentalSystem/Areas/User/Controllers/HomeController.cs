using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Presentation.Areas.User.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.User.Controllers;

[Area("User")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBrandService _brandService;
    private readonly IVehicleService _vehicleService;
    private readonly IImageService _imageService;

    public HomeController(ILogger<HomeController> logger,
        IBrandService brandService,
        IVehicleService vehicleService,
        IImageService imageService)
    {
        _logger = logger;
        _brandService = brandService;
        _vehicleService = vehicleService;
        _imageService = imageService;
    }

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
                        join image in _imageService.GetAllImages()
                           on vehicle.Id equals image.VehicleId
                        select new VehiclesViewModel
                        {
                            Id = vehicle.Id,
                            Name = $"{vehicle.Model} {_brandService.GetBrand(vehicle.BrandId).Name}",
                            Image = image.ProfileImage,
							ImageURL = image.ImageURL,
							PricePerDay = $"Rs {vehicle.PricePerDay}/day"
                        }).DistinctBy(x => x.Id).ToList();

        var details = new HomeViewModel()
        {
            Brand = brands,
            Vehicle = vehicles,
        };

        return View(details);
    }

    public IActionResult Detail(Guid id)
    {
        var vehicle = _vehicleService.GetVehicle(id);

        var result = new GetVehicleViewModel
        {
            Id = id,
            Name = $"{vehicle.Model} - {_brandService.GetBrand(vehicle.BrandId).Name}",
			PlateNumber = vehicle.PlateNumber,
			Images = _imageService.GetVehicleImages(vehicle.Id),
            PricePerDay = $"Rs {vehicle.PricePerDay}/day",
            Description = vehicle.Description,
            Features = vehicle.Features,
            Color = vehicle.Color,
        };

        return View(result);

    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}