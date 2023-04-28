using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddCustomer(Customer customer)
    {
        _unitOfWork.Customer.Add(customer);
        _unitOfWork.Save();
    }

    public List<Customer> GetAllCustomers()
    {
        return _unitOfWork.Customer.GetAll();   
    }

    public Customer GetCustomer(Guid Id)
    {
        return _unitOfWork.Customer.GetFirstOrDefault(x => x.Id == Id);
    }

    public AppUser GetUserCustomer(string Id)
    {
        return _unitOfWork.AppUser.Retrieve(Id);
    }
}
