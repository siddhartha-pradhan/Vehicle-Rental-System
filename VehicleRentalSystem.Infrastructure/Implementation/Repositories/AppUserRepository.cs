using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Infrastructure.Persistence;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class AppUserRepository : Repository<AppUser>, IAppUserRepository
{
    private readonly ApplicationDbContext _dbContext;

	public AppUserRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
        _dbContext = dbContext;
    }
}
