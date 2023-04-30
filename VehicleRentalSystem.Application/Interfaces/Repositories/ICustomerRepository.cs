using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Repositories; 

public interface ICustomerRepository : IRepository<Customer>
{
    void Update(Customer customer);
}
