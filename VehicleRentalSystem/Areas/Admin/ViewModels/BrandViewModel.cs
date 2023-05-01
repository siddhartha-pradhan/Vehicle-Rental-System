using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class BrandViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public byte[] Image { get; set; }

	public string ImageURL { get; set; }


	[Display(Name = "Activation Status")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Date of Creation")]
    public string CreatedDate { get; set; }

    [Display(Name = "Creator User")]
    public string CreatedBy { get; set; }

    [Display(Name = "Date of Modification")]
    public string? LastModifiedDate { get; set; }

    [Display(Name = "Modifier User")]
    public string? LastModifiedBy { get; set; }
}
