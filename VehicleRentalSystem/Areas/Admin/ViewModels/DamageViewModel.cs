namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class DamageViewModel
{
    public Guid DamageId { get; set; }
    
    public string Description { get; set; }
    
    public string RequestDate { get; set; }

    public string ApprovedDate { get; set; }

    public Guid RentalId { get; set; }
    
    public Guid VehicleId { get; set; }

    public string VehicleName { get; set; }
    
    public string CustomerId { get; set; }
    
    public string CustomerName { get; set; }
    
    public string CustomerPhone { get; set; }
    
    public string ApproverId { get; set; }
    
    public string ApproverName { get; set; }
    
    public string PaymentStatus { get; set; }
    
    public string Cost { get; set; }

    public double RepairCost { get; set; }

    public string DamageImage { get; set; }
}
