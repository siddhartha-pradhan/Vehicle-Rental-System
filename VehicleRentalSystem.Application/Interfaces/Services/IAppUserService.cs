using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IAppUserService
{
    string GetUserName(string email);

    byte[] GetImage(string email);

    AppUser GetUser(string Id);

    List<AppUser> GetAllUsers();

    void LockUser(string Id);

    void UnlockUser(string Id);
}
