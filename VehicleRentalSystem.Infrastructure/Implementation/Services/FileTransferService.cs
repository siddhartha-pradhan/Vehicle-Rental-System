using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Constants;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class FileTransferService : IFileTransferService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileTransferService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string FilePath(IFormFile file, string type, string name, string? role)
    {
        var fileName = "";
        var path = $@"images\{type}\";

        var wwwRootPath = _webHostEnvironment.WebRootPath;

        var random = new Random();
        var stringChars = new char[8];
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        var finalString = new String(stringChars);


        if (type == Constants.Vehicle)
        {
            fileName = $"[Vehicle - {finalString}] {name} - Image";
        }
        else if (type == Constants.User)
        {
            fileName = $"[{role} - {finalString}] {name} - Image";
        }
        else if (type == Constants.Brand)
        {
            fileName = $"[Brand - {finalString}] {name} - Image";
        }
        else if (type == Constants.Vehicle)
        {
            fileName = $"[Vehicle - {finalString}] {name} - Image";
        }
        else if (type == Constants.Citizenship.ToLower())
        {
            fileName = $"[Citizenship - {finalString}] {name} - Image";
        }
        else if (type == Constants.Licenses.ToLower())
        {
            fileName = $"[License - {finalString}] {name} - Image";
        }
        else if (type == Constants.Damages.ToLower())
        {
            fileName = $"[Damage - {finalString}] {name} - Image";
        }

        var uploads = Path.Combine(wwwRootPath, path);

        var extension = Path.GetExtension(file.FileName);

        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
        {
            file.CopyTo(fileStreams);
        }

        return @$"images\{type.ToLower()}\" + fileName + extension;
    }

    public byte[] ImageByte(IFormFile file)
    {
        using (var dataStream = new MemoryStream())
        {
            file.CopyToAsync(dataStream);

            return dataStream.ToArray();
        }
    }
}
