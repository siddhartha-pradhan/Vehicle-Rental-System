using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class DamageRequestService : IDamageRequestService
{
    private readonly IUnitOfWork _unitOfWork;

    public DamageRequestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddDamageRequest(DamageRequest request)
    {
        _unitOfWork.DamageRequest.Add(request);
        _unitOfWork.Save();
    }

    public DamageRequest DamageRequest(Guid Id)
    {
        return _unitOfWork.DamageRequest.Get(Id);
    }

    public List<DamageRequest> GetAllDamageRequests()
    {
        return _unitOfWork.DamageRequest.GetAll();
    }
}
