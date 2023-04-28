namespace VehicleRentalSystem.Domain.Shared;

public class BaseEntity
{
    public DateTime CreatedDate { get; set; }
    
    public string CreatedBy { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public string? LastModifiedBy { get; set; } 

    public DateTime? DeletedDate { get; set; }

    public string? DeletedBy { get; set; }

    public bool IsDeleted { get; set; } = false;
}
