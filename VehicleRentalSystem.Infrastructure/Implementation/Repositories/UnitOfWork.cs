using VehicleRentalSystem.Infrastructure.Persistence;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        AppUser = new AppUserRepository(_dbContext);
        Customer = new CustomerRepository(_dbContext);
        Staff = new StaffRepository(_dbContext);
        Vehicle = new VehicleRepository(_dbContext);
    }

    public IAppUserRepository AppUser { get; set; }
    
    public ICustomerRepository Customer { get; set; }

    public IDamageRequestRepository DamageRequest { get; set; }

    public IRentalRepository Rental { get; set; }

    public IOfferRepository Offer { get; set; }

    public IStaffRepository Staff { get; set; }
    
    public IVehicleRepository Vehicle { get; set; }
    
    public void Save()
    {
        _dbContext.SaveChanges();
    }
}
