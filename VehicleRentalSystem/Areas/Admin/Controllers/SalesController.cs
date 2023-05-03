using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{Constants.Admin}, {Constants.Staff}")]
public class SalesController : Controller
{
    #region Service Injection
    private readonly IAppUserService _appUserService;
    private readonly ICustomerService _customerService;
    private readonly IRentalService _rentalService;
    private readonly IDamageRequestService _damageRequestService;
    private readonly IVehicleService _vehicleService;
    private readonly IBrandService _brandService;

    public SalesController(IAppUserService appUserService,
        ICustomerService customerService,
        IRentalService rentalService,
        IDamageRequestService damageRequestService,
        IVehicleService vehicleService,
        IBrandService brandService)
    {
        _appUserService = appUserService;
        _customerService = customerService;
        _rentalService = rentalService;
        _damageRequestService = damageRequestService;
        _vehicleService = vehicleService;
        _brandService = brandService;
    }
    #endregion

    #region Razor Views
    [HttpGet]
    public IActionResult Index()
    {
        var customers = _customerService.GetAllCustomers().Count();
        var rentals = _rentalService.GetAllRentals().Count();
        var requests = _rentalService.GetAllRentals().Where(x => x.RentalStatus == Constants.Requested).ToList().Count();
        var sales = _rentalService.GetAllRentals().Sum(x => x.TotalAmount);

        var vehicleRents = (from brand in _brandService.GetAllBrands()
                            join vehicle in _vehicleService.GetAllVehicles()
                                on brand.Id equals vehicle.BrandId
                            join rental in _rentalService.GetAllRentals()
                                on vehicle.Id equals rental.VehicleId
                            group vehicle by new { vehicle.Id, vehicle.Model, brand.Name }
                                into g
                            select new VehicleRentViewModel()
                            {
                                Count = g.Count(),
                                Vehicle = $"{g.Key.Model} - {g.Key.Name}"
                            }).ToList();

        var userRents = (from user in _appUserService.GetAllUsers()
                         join customer in _customerService.GetAllCustomers()
                            on user.Id equals customer.UserId
                         join rental in _rentalService.GetAllRentals()
                            on user.Id equals rental.UserId
                         group user by new { user.Id, user.FullName } into g
                         select new UserRentViewModel()
                         {
                             Count = g.Count(),
                             User = g.Key.FullName
                         }).ToList();

        var inactiveCustomers = (from customer in _customerService.GetAllCustomers()
                                 join user in _appUserService.GetAllUsers()
                                     on customer.UserId equals user.Id
                                 join rental in _rentalService.GetAllRentals()
                                     on user.Id equals rental.UserId into rentalGroup
                                 where !rentalGroup.Any(x => x.RequestedDate >= DateTime.Now.AddMonths(-3))
                                 select new InActiveCustomerViewModel()
                                 {
                                     CustomerId = customer.Id,
                                     CustomerName = user.FullName,
                                     LastRentedDate = rentalGroup.OrderByDescending(x => x.RequestedDate).Select(x => x.RequestedDate).FirstOrDefault().ToString("dd/MM/yyyy")
                                 }).ToList();

        var brandCounts = (from brand in _brandService.GetAllBrands()
                           join vehicle in _vehicleService.GetAllVehicles()
                             on brand.Id equals vehicle.BrandId
                           group vehicle by brand.Name into g
                           select new BrandVehicleViewModel
                           {
                               Count = g.Count(),
                               Brand = g.Key
                           }).ToList();

        var result = new SalesViewModel()
        {
            CustomersCount = customers,
            PendingRequestsCount = requests,
            TotalRentalCounts = rentals,
            TotalSales = sales,
            BrandVehicleCount = brandCounts,
            UserRentCount = userRents,
            VehicleRentCount = vehicleRents,
            InactiveUserCount = inactiveCustomers
        };

        return View(result);
    }

    [HttpPost]
    public List<BrandVehicleViewModel> BrandVehicles()
    {
        var data = (from brand in _brandService.GetAllBrands()
                    join vehicle in _vehicleService.GetAllVehicles()
                      on brand.Id equals vehicle.BrandId
                    group vehicle by brand.Name into g
                    select new BrandVehicleViewModel
                    {
                        Count = g.Count(),
                        Brand = g.Key
                    }).ToList();
        return data;
    }
    #endregion
}
