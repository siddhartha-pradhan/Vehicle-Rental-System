using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Infrastructure.Persistence;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class DamageRequestRepository : Repository<DamageRequest>, IDamageRequestRepository
{
    private readonly ApplicationDbContext _dbContext;

	public DamageRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
        _dbContext = dbContext;
    }
}
