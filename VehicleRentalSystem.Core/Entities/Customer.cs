using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VehicleRentalSystem.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }

    public string? CitizenshipNumber { get; set; }

    public string? CitizenshipURL { get; set; }

    public string? LicenseNumber { get; set; }

    public string? LicenseURL { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string UserId { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsRegular { get; set; } = false;

    [ValidateNever]
    [ForeignKey("UserId")]
    public AppUser? AppUser { get; set; }

    [ValidateNever]
    public virtual ICollection<Rental>? Rental { get; set; }
}
