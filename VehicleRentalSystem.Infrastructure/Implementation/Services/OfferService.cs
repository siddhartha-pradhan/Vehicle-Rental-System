using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class OfferService : IOfferService
{
    private readonly IUnitOfWork _unitOfWork;

    public OfferService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddOffer(Offer offer)
    {
        _unitOfWork.Offer.Add(offer);
        _unitOfWork.Save();
    }

    public void DeleteOffer(Guid Id)
    {
        var offer = _unitOfWork.Offer.Get(Id);

        if (offer != null)
        {
            _unitOfWork.Offer.Delete(offer);

            _unitOfWork.Save();
        }
    }

    public List<Offer> GetAllOffers()
    {
        return _unitOfWork.Offer.GetAll();
    }

    public Offer GetOffer(Guid Id)
    {
        return _unitOfWork.Offer.Get(Id);
    }

    public Offer RetrieveOffer(Guid? Id)
    {
        return _unitOfWork.Offer.GetItem(Id);
    }

    public void UpdateOffer(Offer offer)
    {
        _unitOfWork.Offer.Update(offer);
        _unitOfWork.Save();
    }
}
