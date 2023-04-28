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
    public Guid CustomerId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public DateTime? ReturnedDate { get; set; }

    public bool IsApproved { get; set; } = false;

    public bool IsReturned { get; set; } = false;

    public bool IsCancelled { get; set; } = false;

    public string RentalStatus { get; set; } = Constants.Constants.Pending;

    public string PaymentStatus { get; set; } = Constants.Constants.Pending;

    public string? ApprovedBy { get; set; }

    [Required]
    public float TotalAmount { get; set; }

    [ValidateNever]
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }

    [ValidateNever]
    [ForeignKey("VehicleId")]
    public Vehicle? Vehicle { get; set; }

    [ValidateNever]
    [ForeignKey("ApprovedBy")]
    public AppUser? ApproverUser { get; set; }
}
