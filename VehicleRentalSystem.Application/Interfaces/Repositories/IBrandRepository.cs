using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Repositories;

public interface IBrandRepository : IRepository<Brand>
{
    void Update(Brand brand);

    void Delete(Brand brand);
}
