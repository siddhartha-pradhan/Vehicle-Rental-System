﻿using VehicleRentalSystem.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VehicleRentalSystem.Domain.Entities;

public class Offer : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public bool IsExpired { get; set; } = false;

    [ValidateNever]
    public virtual ICollection<Vehicle>? Vehicles { get; set; }
}
