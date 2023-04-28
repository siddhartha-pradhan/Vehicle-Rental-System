using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Repositories;

public interface IVehicleRepository : IRepository<Vehicle>
{
    void Update(Vehicle vehicle);

    void Delete(Vehicle vehicle);
}
