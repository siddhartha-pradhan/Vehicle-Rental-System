using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ChartJSCore.Plugins.Zoom;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


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


        Chart verticalBarChart = GenerateVerticalBarChart();
        Chart horizontalBarChart = GenerateHorizontalBarChart();
        Chart lineChart = GenerateLineChart();

        ViewData["VerticalBarChart"] = verticalBarChart;
        ViewData["HorizontalBarChart"] = horizontalBarChart;
        ViewData["LineChart"] = lineChart;

        var inactiveCustomers = (from customer in _customerService.GetAllCustomers()
                                 join user in _appUserService.GetAllUsers()
                                     on customer.UserId equals user.Id
                                 join rental in _rentalService.GetAllRentals()
                                     on user.Id equals rental.UserId 
                                 where rental.RequestedDate <= DateTime.Now.AddMonths(-3)
                                 select new InActiveCustomerViewModel()
                                 {
                                     CustomerId = customer.Id,
                                     CustomerName = user.FullName,
                                     LastRentedDate = rental.RequestedDate.ToString("dd/MM/yyyy")
                                 }).DistinctBy(x => x.CustomerId).ToList();

        var result = new SalesViewModel()
        {
            CustomersCount = customers,
            PendingRequestsCount = requests,
            TotalRentalCounts = rentals,
            TotalSales = sales,
            InactiveUserCount = inactiveCustomers
        };

        return View(result);
    }

    private Chart GenerateVerticalBarChart()
    {
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

        Chart chart = new Chart();
        chart.Type = Enums.ChartType.Bar;

        Data data = new Data();

        data.Labels = new List<string>();

        foreach(var item in vehicleRents)
        {
            data.Labels.Add(item.Vehicle);
        }

        var counts = new List<double?>();

        foreach (var item in vehicleRents)
        {
            counts.Add(item.Count);
        }

        BarDataset dataset = new BarDataset()
        {
            Label = "Rent Count",
            Data = counts,
            BackgroundColor = new List<ChartColor>
                {
                    ChartColor.FromRgba(255, 99, 132, 0.2),
                },
            BorderColor = new List<ChartColor>
                {
                    ChartColor.FromRgb(255, 99, 132),
                },
            BorderWidth = new List<int>() { 1 },
            BarPercentage = 0.5,
            BarThickness = 6,
            MaxBarThickness = 8,
            MinBarLength = 2
        };

        data.Datasets = new List<Dataset>();
        data.Datasets.Add(dataset);

        chart.Data = data;

        var options = new Options
        {
            Scales = new Dictionary<string, Scale>()
                {
                    { "y", new CartesianLinearScale()
                        {
                            BeginAtZero = true
                        }
                    },
                    { "x", new Scale()
                        {
                            Grid = new Grid()
                            {
                                Offset = true
                            }
                        }
                    },
                }
        };

        chart.Options = options;

        chart.Options.Layout = new Layout
        {
            Padding = new Padding
            {
                PaddingObject = new PaddingObject
                {
                    Left = 10,
                    Right = 12
                }
            }
        };

        return chart;
    }

    private Chart GenerateHorizontalBarChart()
    {
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

        var dataRents = new List<Dataset>();

        foreach(var rent in userRents)
        {
            int i = 0;
            i++;
            var dataSet = new VerticalBarDataset()
            {
                Label = rent.User,
                Data = new List<VerticalBarDataPoint?>()
                        {
                            new VerticalBarDataPoint(rent.Count, i)
                        },
                BackgroundColor = new List<ChartColor>
                            {
                                ChartColor.FromRgba(255, 99, 132, 0.2)
                            },
                BorderWidth = new List<int>() { 2 },
                IndexAxis = "y"
            };
            dataRents.Add(dataSet);
        }

        Chart chart = new Chart();
        chart.Type = Enums.ChartType.Bar;

        chart.Data = new Data()
        {
            Datasets = dataRents
        };

        chart.Options = new Options()
        {
            Responsive = true,
            Plugins = new ChartJSCore.Models.Plugins()
            {
                Legend = new Legend()
                {
                    Position = "right"
                },
                Title = new Title()
                {
                    Display = true,
                    Text = new List<string>() { "Total User Rents" }
                }
            }
        };

        return chart;
    }

    private Chart GenerateLineChart()
    {
        var brandCounts = (from brand in _brandService.GetAllBrands()
                           join vehicle in _vehicleService.GetAllVehicles()
                             on brand.Id equals vehicle.BrandId
                           group vehicle by brand.Name into g
                           select new BrandVehicleViewModel
                           {
                               Count = g.Count(),
                               Brand = g.Key
                           }).ToList();

        var brandData = new List<string>();
        var brandVehicles = new List<double?>();

        foreach (var brand in brandCounts)
        {
            brandData.Add(brand.Brand);
            brandVehicles.Add(brand.Count);
        }

        Chart chart = new Chart();

        chart.Type = Enums.ChartType.Line;
        chart.Options.Scales = new Dictionary<string, Scale>();
        CartesianScale xAxis = new CartesianScale();
        xAxis.Display = true;
        xAxis.Title = new Title
        {
            Text = new List<string> { "Brand Vehicle Count" },
            Display = true
        };
        chart.Options.Scales.Add("x", xAxis);


        Data data = new Data
        {
            Labels = brandData
        };

        LineDataset dataset = new LineDataset()
        {
            Label = "Brand Counts",
            Data = brandVehicles,
            Fill = "true",
            Tension = .01,
            BackgroundColor = new List<ChartColor> { ChartColor.FromRgba(75, 192, 192, 0.4) },
            BorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
            BorderCapStyle = "butt",
            BorderDash = new List<int>(),
            BorderDashOffset = 0.0,
            BorderJoinStyle = "miter",
            PointBorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
            PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
            PointBorderWidth = new List<int> { 1 },
            PointHoverRadius = new List<int> { 5 },
            PointHoverBackgroundColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
            PointHoverBorderColor = new List<ChartColor> { ChartColor.FromRgb(220, 220, 220) },
            PointHoverBorderWidth = new List<int> { 2 },
            PointRadius = new List<int> { 1 },
            PointHitRadius = new List<int> { 10 },
            SpanGaps = false
        };

        data.Datasets = new List<Dataset>
            {
                dataset
            };

        chart.Data = data;

        ZoomOptions zoomOptions = new ZoomOptions
        {
            Zoom = new Zoom
            {
                Wheel = new Wheel
                {
                    Enabled = true
                },
                Pinch = new Pinch
                {
                    Enabled = true
                },
                Drag = new Drag
                {
                    Enabled = true,
                    ModifierKey = Enums.ModifierKey.alt
                }
            },
            Pan = new Pan
            {
                Enabled = true,
                Mode = "xy"
            }
        };

        chart.Options.Plugins = new ChartJSCore.Models.Plugins
        {
            PluginDynamic = new Dictionary<string, object> { { "zoom", zoomOptions } }
        };

        return chart;
    }

    #endregion
}
