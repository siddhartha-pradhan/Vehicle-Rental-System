using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Presentation.Areas.User.ViewModels;

public class DamageRequestViewModel
{
	public string VehicleName { get; set; }

	public DamageRequest DamageRequest { get; set; }
}
