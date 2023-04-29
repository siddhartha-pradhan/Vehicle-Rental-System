using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class BrandService : IBrandService
{
    private readonly IUnitOfWork _unitOfWork;

    public BrandService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddBrand(Brand brand)
    {
        _unitOfWork.Brand.Add(brand);
        _unitOfWork.Save();
    }

    public void DeleteBrand(Guid Id)
    {
        var brand = _unitOfWork.Brand.Get(Id);

        _unitOfWork.Brand.Delete(brand);
        _unitOfWork.Save();
    }

    public List<Brand> GetAllBrands()
    {
        return _unitOfWork.Brand.GetAll();
    }

    public Brand GetBrand(Guid Id)
    {
        return _unitOfWork.Brand.GetFirstOrDefault(x => x.Id == Id);
    }

    public void UpdateBrand(Brand brand)
    {
        _unitOfWork.Brand.Update(brand);
        _unitOfWork.Save(); 
    }
}
