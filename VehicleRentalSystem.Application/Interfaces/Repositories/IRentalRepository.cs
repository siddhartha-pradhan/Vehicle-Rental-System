using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Repositories; 

public interface IRentalRepository : IRepository<Rental> 
{ 
    void Delete(Rental rental);
}