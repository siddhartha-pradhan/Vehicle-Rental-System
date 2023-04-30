using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Infrastructure.Persistence;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    private readonly ApplicationDbContext _dbContext;

	public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
        _dbContext = dbContext;
    }
}
