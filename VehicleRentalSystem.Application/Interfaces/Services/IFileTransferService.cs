using Microsoft.AspNetCore.Http;

namespace VehicleRentalSystem.Application.Interfaces.Services;

public interface IFileTransferService
{
    string FilePath(IFormFile file, string type, string name, string? role);

    byte[] ImageByte(IFormFile file);
}
