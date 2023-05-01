namespace VehicleRentalSystem.Presentation.Areas.User.ViewModels
{
    public class VehiclesViewModel
    {
        public Guid Id { get; set; }

        public byte[] Image { get; set; }

        public string ImageURL { get; set; }

        public string Name { get; set; }
        
        public string? Offer { get; set; }

        public string PricePerDay { get; set; } 
    }
}
