using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using VehicleRentalSystem.Domain.Shared;

namespace VehicleRentalSystem.Domain.Entities;

public class Brand : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public byte[] Image { get; set; }   

    [Required]
    [Display(Name = "Image URL")]
    public string ImageURL { get; set; }

    public bool IsActive { get; set; } = true;

    [ValidateNever]
    public virtual ICollection<Vehicle>? Vehicles { get; set; }
}
