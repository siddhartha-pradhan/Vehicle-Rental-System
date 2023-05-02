using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class RentalDetailsViewModel
{
	public Guid Id { get; set; }

	public string UserId { get; set; }

	public string UserName { get; set; }
	
	public string PhoneNumber { get; set; }

	public string? LicenseNumber { get; set; }

	public string? LicenseURL { get; set; }

	public string? CitizenshipNumber { get; set; }

	public string? CitizenshipURL { get; set; }

	public Guid VehicleId { get; set; }

	public string VehicleName { get; set; }

	public string RequestedDate { get; set; }

	public string StartDate { get; set; }

	public string EndDate { get; set; }

	public string? ActionDate { get; set; }

	public string? ReturnedDate { get; set; }

	public string ActionBy { get; set; }

	public string TotalAmount { get; set; }

    public byte[] Image { get; set; }

    public string ImageURL { get; set; }
}
