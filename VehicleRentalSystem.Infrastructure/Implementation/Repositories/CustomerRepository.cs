using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Infrastructure.Persistence;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
        _dbContext = dbContext;
    }

    public void Update(Customer customer)
    {
        var item = _dbContext.Customers.FirstOrDefault(x => x.Id == customer.Id);   

        if (item != null)
        {
            item.CitizenshipNumber = customer.CitizenshipNumber;
            item.CitizenshipURL = customer.CitizenshipURL;
            item.LicenseNumber = customer.LicenseNumber;
            item.LicenseURL = customer.LicenseURL;
            item.ExpirationDate = customer.ExpirationDate;
        }
    }
}
