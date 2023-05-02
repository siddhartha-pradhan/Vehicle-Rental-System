namespace VehicleRentalSystem.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IAppUserRepository AppUser { get; set; }

    IBrandRepository Brand { get; set; }
    
    ICustomerRepository Customer { get; set; }

    IDamageRequestRepository DamageRequest { get; set; }

    IOfferRepository Offer { get; set; }

    IRentalRepository Rental { get; set; }

    IRoleRepository Role { get; set; }

    IStaffRepository Staff { get; set; }

    IUserRoleRepository UserRole { get; set; }

    IVehicleRepository Vehicle { get; set; }

    void Save();
}
