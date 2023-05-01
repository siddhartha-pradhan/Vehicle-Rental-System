using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels
{
    public class OfferDetailViewModel
    {
        public string Name { get; set; }
        
        public string StartDate { get; set; }
        
        public string EndDate { get; set; }
        
        public string Discount { get; set; }
        
        public string Description { get; set; }
        
        public List<Vehicle> Vehicles { get; set; }
    }
}
