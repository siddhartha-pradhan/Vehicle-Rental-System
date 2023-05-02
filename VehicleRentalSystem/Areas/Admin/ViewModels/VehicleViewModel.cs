using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class VehicleViewModel
{
    public Guid Id { get; set; }

    public byte[] Image { get; set; }

    public string ImageURL { get; set; }

    public string Model { get; set; }

    public string Description { get; set; }

    public string Features { get; set; }

    public string Color { get; set; }

    [Display(Name = "Plate Number")]
    public string PlateNumber { get; set; }

    [Display(Name = "Price per Day")]
    public string PricePerDay { get; set; }

    public string Brand { get; set; }

    public string Offer { get; set; }

    public string Availablility { get; set; }

    public string Rented { get; set; }

    public int TotalRents { get; set; }

    [Display(Name = "Creator User")]
    public string CreatedBy { get; set; }

    [Display(Name = "Date of Creation")]
    public string CreatedDate { get; set; }

    [Display(Name = "Date of Modification")]
    public string? LastModifiedDate { get; set; }

    [Display(Name = "Modifier User")]
    public string? LastModifiedBy { get; set; }
}
