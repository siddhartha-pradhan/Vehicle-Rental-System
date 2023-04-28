using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface ICustomerService
{
    Customer GetCustomer(Guid Id);

    AppUser GetUserCustomer(string Id);

    List<Customer> GetAllCustomers();

    void AddCustomer(Customer customer);
}
