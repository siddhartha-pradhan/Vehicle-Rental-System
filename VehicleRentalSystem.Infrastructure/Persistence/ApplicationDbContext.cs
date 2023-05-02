using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleRentalSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace VehicleRentalSystem.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<DamageRequest> DamageRequests { get; set; }

    public DbSet<Offer> Offers { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Rental> Rentals { get; set; }

    public DbSet<Staff> Staffs { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Rental>()
           .HasOne(x => x.Vehicle)
           .WithMany(x => x.Rental)
           .HasForeignKey(p => p.VehicleId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IdentityUser>().ToTable("Users");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("Tokens");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("LoginAttempts");
    }
}