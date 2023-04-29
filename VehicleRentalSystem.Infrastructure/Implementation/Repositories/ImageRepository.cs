using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Infrastructure.Persistence;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class ImageRepository : Repository<Image>, IImageRepository
{
    private readonly ApplicationDbContext _dbContext;

	public ImageRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
        _dbContext = dbContext;
    }
}
