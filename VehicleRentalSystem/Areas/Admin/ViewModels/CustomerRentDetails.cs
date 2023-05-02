using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class CustomerRentDetails
{
    public AppUser Customer { get; set; }

    public List<CustomerDetailRentViewModel> CustomerRent { get; set; }
}
