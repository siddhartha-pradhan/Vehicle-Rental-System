using VehicleRentalSystem.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VehicleRentalSystem.Domain.Entities; 

public class Vehicle : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Model { get; set; }

    [Required]
    [MaxLength(50)]
    public string Color { get; set; }

    [Required]
    [MaxLength(50)]
    [Display(Name = "Plate Number")]
    public string PlateNumber { get; set; }

    [Required]
    [Display(Name = "Price per Day")]
    public double PricePerDay { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    [MaxLength(500)]
    public string Features { get; set; }

    [Required]
    [Display(Name = "Brand")]
    public Guid BrandId { get; set; }

    public Guid? OfferId { get; set; }

    [Display(Name = "Availability Status")]
    public bool IsAvailable { get; set; } = true;

    public byte[] Image { get; set; }

    public string ImageURL { get; set; }

    [ForeignKey("BrandId")]
    public Brand? Brand { get; set; }

    [ForeignKey("OfferId")]
    public Offer? Offer { get; set; }

    [ValidateNever]
    public virtual ICollection<Rental>? Rental { get; set; }
}