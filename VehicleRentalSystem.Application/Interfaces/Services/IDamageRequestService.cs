using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IDamageRequestService
{
    DamageRequest DamageRequest(Guid Id);

    List<DamageRequest> GetAllDamageRequests();

    void AddDamageRequest(DamageRequest request);
}
