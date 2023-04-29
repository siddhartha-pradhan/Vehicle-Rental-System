using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Infrastructure.Persistence;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class BrandRepository : Repository<Brand>, IBrandRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BrandRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override List<Brand> FilterDeleted()
    {
        return base.FilterDeleted().Where(x => !x.IsDeleted).ToList();
    }

    public override void Add(Brand brand)
    {
        brand.CreatedDate = DateTime.Now;
        base.Add(brand);
    }

    public void Delete(Brand brand)
    {
        brand.IsDeleted = true;
        brand.DeletedDate = DateTime.Now;
    }

    public void Update(Brand brand)
    {
        var result = _dbContext.Brands.FirstOrDefault(x => x.Id == brand.Id);

        if (result != null)
        {
            result.Name = brand.Name;
            result.Description = brand.Description;
            result.Image = brand.Image;
            result.ImageURL = brand.ImageURL;
            result.IsActive = brand.IsActive;

            result.LastModifiedBy = brand.LastModifiedBy;
            result.LastModifiedDate = DateTime.Now;
        }
    }
}
