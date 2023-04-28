using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VehicleRentalSystem.Domain.Entities;

public class Staff
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    [ValidateNever]
    [ForeignKey("UserId")]
    public AppUser? AppUser { get; set; }
}
