using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class VehicleService : IVehicleService
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddVehicle(Vehicle vehicle)
    {
        _unitOfWork.Vehicle.Add(vehicle);
        _unitOfWork.Save();
    }

    public void DeleteVehicle(Guid Id)
    {
        var vehicle = _unitOfWork.Vehicle.Get(Id);
        
        if (vehicle != null)
        {
            _unitOfWork.Vehicle.Delete(vehicle);
            _unitOfWork.Save();
        }
    }

    public List<Vehicle> GetAllVehicles()
    {
        return _unitOfWork.Vehicle.GetAll();
    }

    public Vehicle GetVehicle(Guid Id)
    {
        return _unitOfWork.Vehicle.Get(Id);
    }

    public void UpdateVehicle(Vehicle vehicle)
    {
        _unitOfWork.Vehicle.Update(vehicle);
        _unitOfWork.Save();
    }
}
