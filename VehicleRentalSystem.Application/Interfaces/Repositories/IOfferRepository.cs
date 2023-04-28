using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Repositories;

public interface IOfferRepository : IRepository<Offer>
{
    void Update(Offer offer);

    void Delete(Offer offer);
}
