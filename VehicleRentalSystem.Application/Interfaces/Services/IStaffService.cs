using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IStaffService
{
    Staff GetStaff(Guid Id);

    AppUser GetUserStaff(string Id);

    List<Staff> GetAllStaffs();

    void AddStaff(Staff staff);
}