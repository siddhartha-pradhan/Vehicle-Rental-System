using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class AppUserService : IAppUserService
{
    private readonly IUnitOfWork _unitOfWork;

	public AppUserService(IUnitOfWork unitOfWork)
	{
        _unitOfWork = unitOfWork;
    }

    public List<AppUser> GetAllUsers()
    {
        return _unitOfWork.AppUser.GetAll();
    }

    public byte[] GetImage(string email)
    {
        return _unitOfWork.AppUser.GetAll().Where(x => x.Email == email).FirstOrDefault().Image;
    }

    public AppUser GetUser(string Id)
    {
        return _unitOfWork.AppUser.Retrieve(Id);
    }

    public string GetUserName(string email)
    {
        return _unitOfWork.AppUser.GetAll().Where(x => x.Email == email).FirstOrDefault().FullName;
    }

    public void LockUser(string Id)
    {
        var user = _unitOfWork.AppUser.GetFirstOrDefault(x => x.Id == Id);

        if (user != null)
        {
            user.LockoutEnabled = true;

            user.LockoutEnd = DateTime.Now.AddDays(5);
        }

        _unitOfWork.Save();
    }

    public void UnlockUser(string Id)
    {
        var user = _unitOfWork.AppUser.GetFirstOrDefault(x => x.Id == Id);

        if (user != null)
        {
            user.LockoutEnabled = false;
            
            user.LockoutEnd = DateTime.Now;
        }

        _unitOfWork.Save();
    }
}
