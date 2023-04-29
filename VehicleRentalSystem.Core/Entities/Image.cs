using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VehicleRentalSystem.Domain.Entities;

public class Image
{
    public Guid Id { get; set; }

    public byte[] ProfileImage { get; set; }

    public string ImageURL { get; set; }

    public Guid VehicleId { get; set; }

    [ValidateNever]
    [ForeignKey("VehicleId")]
    public Vehicle? Vehicle { get; set; }
}
