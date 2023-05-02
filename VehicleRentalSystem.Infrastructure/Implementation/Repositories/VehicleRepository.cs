using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Infrastructure.Persistence;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
{
    private readonly ApplicationDbContext _dbContext;

    public VehicleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override List<Vehicle> FilterDeleted()
    {
        return base.FilterDeleted().Where(x => !x.IsDeleted).ToList();
    }

    public override void Add(Vehicle vehicle)
    {
        vehicle.CreatedDate = DateTime.Now;
        base.Add(vehicle);
    }

    public void Delete(Vehicle vehicle)
    {
        vehicle.IsDeleted = true;
        vehicle.DeletedDate = DateTime.Now;
    }

    public void Update(Vehicle vehicle)
    {
        var item = _dbContext.Vehicles.FirstOrDefault(x => x.Id == vehicle.Id); 
        
        if (item != null)
        {
            item.Model = vehicle.Model;
            item.Brand = vehicle.Brand;
            item.Color = vehicle.Color;
            item.Features = vehicle.Features;
            item.PricePerDay = vehicle.PricePerDay;
            item.Description = vehicle.Description;
            item.PlateNumber = vehicle.PlateNumber;
            item.IsAvailable = vehicle.IsAvailable;
            
            item.LastModifiedBy = vehicle.LastModifiedBy;
            item.LastModifiedDate = DateTime.Now;
        }
    }
}
