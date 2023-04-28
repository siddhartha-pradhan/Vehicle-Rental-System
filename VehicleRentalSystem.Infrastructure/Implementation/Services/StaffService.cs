using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

internal class StaffService : IStaffService
{
    private readonly IUnitOfWork _unitOfWork;

    public StaffService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddStaff(Staff staff)
    {
        _unitOfWork.Staff.Add(staff);
        _unitOfWork.Save();
    }

    public List<Staff> GetAllStaffs()
    {
        return _unitOfWork.Staff.GetAll();
    }

    public Staff GetStaff(Guid Id)
    {
        return _unitOfWork.Staff.Get(Id);
    }

    public AppUser GetUserStaff(string Id)
    {
        return _unitOfWork.AppUser.Retrieve(Id);
    }
}
