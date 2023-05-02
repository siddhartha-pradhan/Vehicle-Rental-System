using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VehicleRentalSystem.Domain.Entities;

public class DamageRequest
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid RentalId { get; set; }

    [Display(Name = "Repair Cost")]
    public double? RepairCost { get; set; }

	[Required]
    [Display(Name = "Damage Description")]
    public string DamageDescription { get; set; }

    public bool IsPaid { get; set; } = false;

    [Display(Name = "Damage Requested Date")]
    public DateTime DamageRequestDate { get; set; } = DateTime.Now;

	public string? ApprovedBy { get; set; }
	
    public DateTime? ActionDate { get; set; }

	[Required]
    public bool IsApproved { get; set; } = false;

    public string ImageURL { get; set; }

    [ValidateNever]
    [ForeignKey("RentalId")]
    public Rental? Rental { get; set; }

    [ValidateNever]
    [ForeignKey("ApprovedBy")]
    public AppUser? Approver { get; set; }
}
