namespace VehicleRentalSystem.Presentation.Areas.User.ViewModels
{
	public class DamageDetailViewModel
	{
		public double? RepairCost { get; set; }

		public string DamageDescription { get; set; }

		public bool IsPaid { get; set; } = false;

		public DateTime DamageRequestDate { get; set; } 
	}
}
