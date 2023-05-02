using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.Admin)]
public class OfferController : Controller
{
    #region Service Injection
    private readonly IOfferService _offerService;
    private readonly IVehicleService _vehicleService;
    private readonly IUnitOfWork _unitOfWork;

    public OfferController(IOfferService offerService, 
        IVehicleService vehicleService,
        IUnitOfWork unitOfWork)
    {
        _offerService = offerService;
        _vehicleService = vehicleService;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Razor Views
    [HttpGet]
    public IActionResult Index()
    {
        var offers = _offerService.GetAllOffers();
        return View(offers);
    }

    public IActionResult Detail(Guid id)
    {
        var offer = _offerService.GetOffer(id);

        var result = new OfferDetailViewModel()
        {
            Name = offer.Name,
            StartDate = offer.StartDate.ToString("dd/MM/yyyy"),
            EndDate = offer.EndDate.ToString("dd/MM/yyyy"),
            Discount = $"{offer.Discount}%",
            Description = offer.Description,
            Vehicles = _vehicleService.GetAllVehicles().Where(x => x.OfferId == offer.Id).ToList()
        };

        return View(result);
    }

    public IActionResult Insert()
    {
        var offer = new OfferViewModel();

        var vehicles = _vehicleService.GetAllVehicles()
            .Select(x => new SelectListItem()
            {
                Text = x.Model,
                Value = x.Id.ToString()
            }).ToList();

        offer.Vehicles = vehicles;

        return View(offer);
    }

    public IActionResult Delete(Guid id)
    {
        var offer = _offerService.GetOffer(id);

        if (offer != null)
        {
            return View(offer);
        }

        return NotFound();
    }
    #endregion

    #region API Calls
    [HttpPost]
    public IActionResult Insert(OfferViewModel offerViewModel)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;

        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        var offerId = Guid.NewGuid();

        var item = new Offer()
        {
            Id = offerId,
            Name = offerViewModel.Offer.Name,
            StartDate = offerViewModel.Offer.StartDate,
            EndDate = offerViewModel.Offer.EndDate,
            Discount = offerViewModel.Offer.Discount,
            Description = offerViewModel.Offer.Description.Replace("<p>", "").Replace("</p>", ""),
            CreatedBy = claim.Value,
        };

        _offerService.AddOffer(item);

        foreach(var vehicle in offerViewModel.VehicleList)
        {
            var result = _vehicleService.GetVehicle(vehicle);
            result.OfferId = offerId;
        }

        _unitOfWork.Save();

        TempData["Success"] = "Offer successfully added";

        return RedirectToAction("Index");
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteBrand(Guid id)
    {
        var offer = _offerService.GetOffer(id);


        if (offer != null)
        {
            var vehicles = _vehicleService.GetAllVehicles().Where(x => x.OfferId == offer.Id).ToList();

            foreach(var vehicle in vehicles)
            {
                vehicle.OfferId = null;
                
                _unitOfWork.Save();
            }

            _offerService.DeleteOffer(offer.Id);

            TempData["Delete"] = "Offer successfully deleted";

            return RedirectToAction("Index");

        }

        return NotFound();
    }
    #endregion
}
