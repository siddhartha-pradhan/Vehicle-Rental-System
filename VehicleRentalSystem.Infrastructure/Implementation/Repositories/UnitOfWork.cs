using VehicleRentalSystem.Infrastructure.Persistence;
using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        AppUser = new AppUserRepository(_dbContext);
        Brand = new BrandRepository(_dbContext);
        Customer = new CustomerRepository(_dbContext);
        DamageRequest = new DamageRequestRepository(_dbContext);
        Offer = new OfferRepository(_dbContext);
        Role = new RoleRepository(_dbContext);  
        Rental = new RentalRepository(_dbContext);
        Staff = new StaffRepository(_dbContext);
        UserRole = new UserRoleRepository(_dbContext);
        Vehicle = new VehicleRepository(_dbContext);
    }

    public IAppUserRepository AppUser { get; set; }

    public IBrandRepository Brand { get; set; }

    public ICustomerRepository Customer { get; set; }

    public IDamageRequestRepository DamageRequest { get; set; }

    public IOfferRepository Offer { get; set; }

    public IRentalRepository Rental { get; set; }

    public IRoleRepository Role { get; set; }

    public IStaffRepository Staff { get; set; }

    public IUserRoleRepository UserRole { get; set; }

    public IVehicleRepository Vehicle { get; set; }
    
    public void Save()
    {
        _dbContext.SaveChanges();
    }
}
