using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IBrandService
{
    Brand GetBrand(Guid Id);

    List<Brand> GetAllBrands();

    void AddBrand(Brand brand);

    void UpdateBrand(Brand brand);

    void DeleteBrand(Guid Id);
}
