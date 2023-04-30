using VehicleRentalSystem.Infrastructure.Dependency;
using VehicleRentalSystem.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var configurations = builder.Configuration;

services.AddInfrastructure(configurations);

services.AddControllersWithViews();

services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/User/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{Area=User}/{controller=Home}/{action=Index}/{id?}");

<<<<<<< HEAD
using (var scope = app.Services.CreateScope())
=======
//SeedDatabase();

app.Run();

void SeedDatabase()
>>>>>>> 6b0c59321350b3cbeda7da1fe1d310162df4feef
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

    dbInitializer.Initialize();
}

app.Run();