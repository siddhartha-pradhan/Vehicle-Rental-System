using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Infrastructure.Persistence;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class RentalRepository : Repository<Rental>, IRentalRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RentalRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Delete(Rental rental)
    {
        _dbContext.Rentals.Remove(rental);
    }
}
