using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Infrastructure.Persistence;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
