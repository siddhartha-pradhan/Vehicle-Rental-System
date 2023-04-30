using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Domain.Constants;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class RentalService : IRentalService
{
    private readonly IUnitOfWork _unitOfWork;

    public RentalService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddRental(Rental rental)
    {
        _unitOfWork.Rental.Add(rental);
        _unitOfWork.Save();
    }

    public void DeleteRental(Guid Id)
    {
        var rental = _unitOfWork.Rental.Get(Id);

        if (rental != null)
        {
            _unitOfWork.Rental.Delete(rental);
            _unitOfWork.Save();
        }
    }

    public List<Rental> GetAllRentals()
    {
        return _unitOfWork.Rental.GetAll();
    }

    public Rental GetRental(Guid Id)
    {
        return _unitOfWork.Rental.Get(Id);
    }

    public void CancelRent(Guid Id)
    {
        var rental = _unitOfWork.Rental.Get(Id);
        var vehicle = _unitOfWork.Vehicle.Get(rental.VehicleId);

        if (rental != null)
        {
            vehicle.IsAvailable = true;

            _unitOfWork.Rental.Delete(rental);
            _unitOfWork.Save();
        }
    }
}
