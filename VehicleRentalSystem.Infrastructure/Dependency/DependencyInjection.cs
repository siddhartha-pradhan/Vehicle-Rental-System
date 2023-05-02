using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleRentalSystem.Application.Interfaces.Repositories;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Infrastructure.Implementation.Repositories;
using VehicleRentalSystem.Infrastructure.Implementation.Services;
using VehicleRentalSystem.Infrastructure.Persistence;
using VehicleRentalSystem.Infrastructure.Persistence.Seed;

namespace VehicleRentalSystem.Infrastructure.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            options.SignIn.RequireConfirmedAccount = true)
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(100);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        services.AddScoped<IDbInitializer, DbInitializer>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IAppUserService, AppUserService>();
        services.AddTransient<IBrandService, BrandService>();
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<IDamageRequestService, DamageRequestService>();
        services.AddTransient<IEmailSender, EmailSenderService>();
        services.AddTransient<IFileTransferService, FileTransferService>();
        services.AddTransient<IOfferService, OfferService>();
        services.AddTransient<IRentalService, RentalService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IStaffService, StaffService>();
        services.AddTransient<IUserRoleService, UserRoleService>();
        services.AddTransient<IVehicleService, VehicleService>();

        return services;
    }
}
