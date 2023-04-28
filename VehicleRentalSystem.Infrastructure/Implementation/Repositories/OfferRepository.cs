using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Infrastructure.Persistence;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class OfferRepository : Repository<Offer>, IOfferRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OfferRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override List<Offer> FilterDeleted()
    {
        return base.FilterDeleted().Where(x => !x.IsDeleted).ToList();
    }

    public override void Add(Offer offer)
    {
        offer.CreatedDate = DateTime.Now;
        base.Add(offer);
    }

    public void Delete(Offer offer)
    {
        offer.IsDeleted = true;
        offer.DeletedDate = DateTime.Now;
    }

    public void Update(Offer offer)
    {
        var result = _dbContext.Offers.FirstOrDefault(x => x.Id == offer.Id);

        if(result != null)
        {
            result.Name = offer.Name;
            result.Discount = offer.Discount;
            result.Description = offer.Description;
            result.StartDate = offer.StartDate;
            result.EndDate = offer.EndDate;

            result.LastModifiedBy = result.LastModifiedBy;
            result.LastModifiedDate = result.LastModifiedDate;
        }
    }
}
