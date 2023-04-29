using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IImageService
{
    void AddImage(Image image);

    void RemoveImage(Guid vehicleId);

    List<Image> GetAllImages();

    List<Image> GetVehicleImages(Guid vehicleId);
}
