using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Presentation.Areas.User.ViewModels;

public class GetVehicleViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string PlateNumber { get; set; }

    public byte[] Image { get; set; }

    public string ImageURL { get; set; }

    public string PricePerDay { get; set; }
    
    public string Description { get; set; }
    
    public string Features { get; set; }

    public string Color { get; set; }

}
