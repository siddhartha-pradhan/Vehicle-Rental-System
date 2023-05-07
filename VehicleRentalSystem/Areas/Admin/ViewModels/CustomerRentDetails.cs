using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class CustomerRentDetails
{
    public AppUser Customer { get; set; }

    public string Role { get; set; }

    public string CustomerStatus { get; set; }

    public List<CustomerDetailRentViewModel> CustomerRent { get; set; }

}
