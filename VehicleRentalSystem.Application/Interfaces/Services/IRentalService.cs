using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IRentalService
{
    Rental GetRental(Guid Id);

    List<Rental> GetAllRentals();

    void AddRental(Rental rental);
    
    void DeleteRental(Guid Id);

    void CancelRent(Guid Id);

}
