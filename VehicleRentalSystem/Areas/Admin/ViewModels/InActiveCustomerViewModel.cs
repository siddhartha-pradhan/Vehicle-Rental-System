namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class InActiveCustomerViewModel
{
    public string UserId { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string LastRentedDate { get; set; }
}