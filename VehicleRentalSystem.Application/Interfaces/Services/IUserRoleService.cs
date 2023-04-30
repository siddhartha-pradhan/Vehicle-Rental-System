using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IUserRoleService
{
    List<UserRole> GetAllUserRoles();
}
