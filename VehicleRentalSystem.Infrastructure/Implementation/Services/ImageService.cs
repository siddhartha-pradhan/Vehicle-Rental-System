using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;

    public ImageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddImage(Image image)
    {
        _unitOfWork.Image.Add(image);
        _unitOfWork.Save();
    }

    public void RemoveImage(Guid vehicleId) 
    {
        var images = _unitOfWork.Image.GetAll().Where(x => x.VehicleId == vehicleId).ToList();

        _unitOfWork.Image.RemoveRange(images);
    }

    public List<Image> GetAllImages()
    {
        return _unitOfWork.Image.GetAll();
    }

    public List<Image> GetVehicleImages(Guid vehicleId)
    {
        var images = _unitOfWork.Image.GetAll().Where(x => x.VehicleId == vehicleId)
                    .Select(x => new Image
                    {
                        Id = x.Id,
                        VehicleId = x.VehicleId,
                        ImageURL = x.ImageURL,
                        ProfileImage = x.ProfileImage,
                    }).ToList();

        return images;
    }
}
