namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class SalesViewModel
{
    public int CustomersCount { get; set;}

    public int TotalRentalCounts { get; set; }

    public int PendingRequestsCount { get; set;}

    public double TotalSales { get; set; }

    public List<BrandVehicleViewModel> BrandVehicleCount { get; set; }  

    public List<VehicleRentViewModel> VehicleRentCount { get; set; }

    public List<UserRentViewModel> UserRentCount { get; set; }

    public List<InActiveCustomerViewModel> InactiveUserCount { get; set; }

}
