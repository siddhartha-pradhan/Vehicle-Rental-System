namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class VehicleDetailRentViewModel
{
    public string CustomerName { get; set; }
    
    public double RentedDays { get; set; }  

    public string ReturnedDate { get; set; }

    public string TotalAmount { get; set; } 

    public string ApprovedStaff { get; set; }
}
