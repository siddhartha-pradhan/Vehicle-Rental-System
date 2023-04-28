using Microsoft.AspNetCore.Identity;

namespace VehicleRentalSystem.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }

    public byte[] Image { get; set; }   

    public string ImageURL { get; set; }

    public string Address { get; set; }

    public string State { get; set; }
}
