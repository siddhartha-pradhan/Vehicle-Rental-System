using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public List<Role> GetAllRoles()
    {
        return _unitOfWork.Role.GetAll();
    }

    public Role GetRole(string id)
    {
        return _unitOfWork.Role.Retrieve(id);
    }
}
