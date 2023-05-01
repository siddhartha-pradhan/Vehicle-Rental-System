using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VehicleRentalSystem.Domain.Entities;

public class Rental
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid VehicleId { get; set; }

    [Required]
    public string UserId { get; set; }

    public DateTime RequestedDate { get; set; } = DateTime.Now;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public DateTime? ReturnedDate { get; set; }

    public DateTime? ActionDate { get; set; }

    public bool IsDamaged { get; set; } = false;

    public bool IsApproved { get; set; } = false;

    public bool IsReturned { get; set; } = false;

    public string RentalStatus { get; set; } = Constants.Constants.Requested;

    public string PaymentStatus { get; set; } = Constants.Constants.Pending;

    public string? ActionBy { get; set; }

    [Required]
    public float TotalAmount { get; set; }

    [ValidateNever]
    [ForeignKey("UserId")]
    public AppUser? AppUser { get; set; }

    [ValidateNever]
    [ForeignKey("VehicleId")]
    public Vehicle? Vehicle { get; set; }

    [ValidateNever]
    [ForeignKey("ActionBy")]
    public AppUser? ApproverUser { get; set; }
}
