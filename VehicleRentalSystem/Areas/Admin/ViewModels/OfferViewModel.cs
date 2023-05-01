using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

public class OfferViewModel
{
    public Offer Offer { get; set; }

    public List<SelectListItem> Vehicles { get; set; }

    public List<Guid> VehicleList { get; set; }
}
