using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services; 

public interface IOfferService 
{
    Offer GetOffer(Guid Id);

    Offer RetrieveOffer(Guid? Id);

    List<Offer> GetAllOffers();

    void AddOffer(Offer offer);

    void UpdateOffer(Offer offer);

    void DeleteOffer(Guid Id);
}