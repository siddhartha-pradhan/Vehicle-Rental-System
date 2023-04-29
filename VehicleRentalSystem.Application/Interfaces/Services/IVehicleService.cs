using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IVehicleService
{
    Vehicle GetVehicle(Guid Id);

    List<Vehicle> GetAllVehicles();

    void AddVehicle(Vehicle vehicle);

    void UpdateVehicle(Vehicle vehicle);

    void DeleteVehicle(Guid Id);
}
