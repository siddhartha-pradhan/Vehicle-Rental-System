using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IRoleService
{
    Role GetRole(string id);

    List<Role> GetAllRoles();
}
