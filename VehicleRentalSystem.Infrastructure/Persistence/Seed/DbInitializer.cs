using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Domain.Constants;

namespace VehicleRentalSystem.Infrastructure.Persistence.Seed;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(ApplicationDbContext dbContext, 
        UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Initialize()
    {
        if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            await _dbContext.Database.MigrateAsync();
        }

        if (!_roleManager.RoleExistsAsync(Constants.Admin).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(Constants.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constants.Staff)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constants.Customer)).GetAwaiter().GetResult();
        }

        var user = new AppUser
        {
            FullName = "Affinity IO",
            Email = "affinity@affinity.io",
            NormalizedEmail = "AFFINITY@AFFINITY.IO",
            UserName = "affinity@affinity.io",
            NormalizedUserName = "AFFINITY@AFFINITY.IO",
            Address = "Harold Street",
            State = "State Somewhere",
            PhoneNumber = "9803364638",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        _userManager.CreateAsync(user, "@ff!N1ty").GetAwaiter().GetResult();

        _userManager.AddToRoleAsync(user, Constants.Admin).GetAwaiter().GetResult();

        await _dbContext.SaveChangesAsync();
    }
}
