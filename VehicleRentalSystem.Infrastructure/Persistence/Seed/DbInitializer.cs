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
        try
        {
            if (_dbContext.Database.GetPendingMigrations().Count() > 0)
            {
                _dbContext.Database.Migrate();
            }
        }
        catch (Exception)
        {
            throw;
        }

        if (!_roleManager.RoleExistsAsync(Constants.Admin).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(Constants.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constants.Staff)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constants.Customer)).GetAwaiter().GetResult();
        }

        var user = new AppUser
        {
            FullName = "Sakshi Gupta",
            Email = "sakshi.gupta1214+admin@gmail.com",
            NormalizedEmail = "SAKSHI.GUPTA1214.+ADMIN@GMAIL.COM",
            UserName = "sakshi.gupta1214+admin@gmail.com",
            NormalizedUserName = "SAKSHI.GUPTA1214.+ADMIN@GMAIL.COM",
            Address = "Harold Street",
            State = "State Somewhere",
            PhoneNumber = "9803364638",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        var userManager = _userManager.CreateAsync(user, "@ff!N1ty").GetAwaiter().GetResult();

        var result = _dbContext.Users.FirstOrDefault(u => u.Email == "sakshi.gupta1214+admin@gmail.com");

        _userManager.AddToRoleAsync(user, Constants.Admin).GetAwaiter().GetResult();

        await _dbContext.SaveChangesAsync();
    }
}
