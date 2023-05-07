using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Domain.Constants;
using Microsoft.AspNetCore.Identity.UI.Services;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.Admin)]
public class UserController : Controller
{
    #region Service Injection
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailSender _emailSender;
    private readonly IStaffService _staffService;
    private readonly IFileTransferService _fileService;
    private readonly IAppUserService _appUserService;
    private readonly IRoleService _roleService;
    private readonly IUserRoleService _userRoleService;
    private readonly ICustomerService _customerService;
    private readonly IRentalService _rentalService;
    private readonly IVehicleService _vehicleService;
    private readonly IBrandService _brandService;
    private readonly IUnitOfWork _unitOfWork;

    public UserController(UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager, IEmailSender emailSender, 
        IStaffService staffService, IFileTransferService fileService, 
        IAppUserService appUserService, 
        IRoleService roleService, 
        IUserRoleService userRoleService,
        ICustomerService customerService,
        IRentalService rentalService,
        IVehicleService vehicleService,
        IBrandService brandService,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _emailSender = emailSender;
        _staffService = staffService;
        _fileService = fileService;
        _appUserService = appUserService;
        _roleService = roleService;
        _userRoleService = userRoleService;
        _customerService = customerService;
        _rentalService = rentalService;
        _vehicleService = vehicleService;
        _brandService = brandService;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Razor Views
    /// <summary>
    /// Defining a view action for admin to view an index of all customers
    /// </summary>
    [HttpGet]
    public IActionResult Customer()
    {
        var users = _appUserService.GetAllUsers();
        var customers = _customerService.GetAllCustomers();
        var rentals = _rentalService.GetAllRentals();

        var result = (from customer in customers
                      join user in users
                        on customer.UserId equals user.Id
                      join rental in rentals
                        on user.Id equals rental.UserId into userRentals
                      from userRental in userRentals.DefaultIfEmpty()
                      group userRental by new { user,customer } into rentalGroup
                      select new UserViewModel
                      {
                          UserId = rentalGroup.Key.user.Id,
                          Name = rentalGroup.Key.user.FullName,
                          Email = rentalGroup.Key.user.Email,
                          Address = rentalGroup.Key.user.Address,
                          State = rentalGroup.Key.user.State,
                          PhoneNumber = rentalGroup.Key.user.PhoneNumber,
                          TotalRents = rentalGroup.Count(x => x != null),
                          LastRentedDate = rentalGroup.Max(x => x != null ? x.RequestedDate.ToString("dd/MM/yyyy") : "Not rented yet"),
                          ActivationStatus = rentalGroup.Key.customer.IsActive == true ? "Active": "Inactive",
                          RegularStatus = rentalGroup.Key.customer.IsRegular == true ? "Regular" : "Normal"
                      }).DistinctBy(x => x.UserId).ToList();

        return View(result);
    }

    /// <summary>
    /// Defining a view action for admin to view a detailed index of a selective customers
    /// </summary>
    [HttpGet]
    public IActionResult Details(string id)
    {
        var user = _appUserService.GetUser(id);

        var customer = _customerService.GetAllCustomers().Where(x => x.UserId == user.Id).FirstOrDefault();

        var role = "Staff";
        var status = "normal";

        if(customer != null)
        {
            role = "Customer";
            status = customer.IsRegular ? "regular" : "normal";
        }

        var rents = (from rent in _rentalService.GetAllRentals().Where(x => x.RentalStatus == Constants.Approved && x.UserId == id)
                     join vehicle in _vehicleService.GetAllVehicles()
                        on rent.VehicleId equals vehicle.Id
                     join staff in _appUserService.GetAllUsers()
                        on rent.ActionBy equals staff.Id
                     select new CustomerDetailRentViewModel()
                     {
                         VehicleName = $"{_vehicleService.GetVehicle(vehicle.Id).Model} - {_brandService.GetBrand(vehicle.BrandId).Name}",
                         RentedDays = (rent.EndDate - rent.StartDate).TotalDays,
                         ReturnedDate = rent.ReturnedDate != null ? rent.ReturnedDate?.ToString("dd/MM/yyyy") : "Not returned yet",
                         ApprovedStaff = staff.FullName,
                         TotalAmount = $"Rs {_rentalService.GetRental(rent.Id).TotalAmount}"
                     }).ToList();

        var result = new CustomerRentDetails()
        {
            Customer = user,
            Role = role,
            CustomerStatus = status,
            CustomerRent = rents
        };

        return View(result);
    }

    [HttpGet]
    public IActionResult System()
    {
        var users = _appUserService.GetAllUsers();
        var roles = _roleService.GetAllRoles();
        var userRoles = _userRoleService.GetAllUserRoles();

        var result = (from user in users
                      join userRole in userRoles
                        on user.Id equals userRole.UserId
                      join role in roles
                        on userRole.RoleId equals role.Id
                      select new UserViewModel
                      {
                          UserId = user.Id,
                          RoleId = role.Id,
                          Name = user.FullName,
                          Email = user.Email,
                          Address = user.Address,
                          State = user.State,
                          PhoneNumber = user.PhoneNumber,
                          Role = role.Name
                      }).ToList();

        return View(result);
    }

    [HttpGet]
    public IActionResult Admin()
    {
        var adminRole = _roleManager.FindByNameAsync(Constants.Admin);
        var adminUsers = _userManager.GetUsersInRoleAsync(adminRole.Result.Name);

        return View(adminUsers);
    }

    [HttpGet]
    public IActionResult Staff()
    {
        var users = _appUserService.GetAllUsers();
        var staffs = _staffService.GetAllStaffs();

        var result = (from user in users
                      join staff in staffs
                        on user.Id equals staff.UserId
                      select new UserViewModel
                      {
                          UserId = user.Id,
                          Name = user.FullName,
                          Email = user.Email,
                          Address = user.Address,
                          State = user.State,
                          PhoneNumber = user.PhoneNumber,
                      }).ToList();

        return View(result);
    }

    [HttpGet]
    public IActionResult Register()
    {
        var staff = new UserViewModel();

        return View(staff);
    }

    [HttpGet]
    public IActionResult Password(string id)
    {
        var user = _appUserService.GetUser(id);

        var userModel = new UserPasswordViewModel()
        {
            Id = id,
            UserName = user.FullName,
            Password = ""
        };

        return View(userModel);
    }
    #endregion

        #region API Calls
        /// <summary>
        /// Defining a post action for admin to register a new staff to the system
        /// </summary>
        [HttpPost]
    public async Task<IActionResult> Register(UserViewModel staff)
    {
        var image = Request.Form.Files.FirstOrDefault();

        var password = Constants.Password;

        var role = staff.Role;

        var user = new AppUser()
        {
            FullName = staff.Name,
            PhoneNumber = staff.PhoneNumber,
            Email = staff.Email,
            Address = staff.Address,
            State = staff.State,
            UserName = staff.Email,
            EmailConfirmed = true,
            Image = _fileService.ImageByte(image),
            ImageURL = _fileService.FilePath(image, Constants.User, staff.Name, Constants.Staff)
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, role);

            var model = new Staff()
            {
                UserId = user.Id
            };

            _staffService.AddStaff(model);
        }

        await _emailSender.SendEmailAsync(staff.Email, "Email Confirmation",
                    $"Dear {user.FullName},<br><br>You have been registered to our system as a {role}. " +
                    $"<br>Please use your registered email and password as <b>\"{password}\"</b> as the login credential for the system." +
                    $"<br><br>Regards,<br>Hajur ko Car Rental");

        TempData["Success"] = "Staff successfully registered";

        return RedirectToAction("Staff");

    }

    [HttpPost]
    public IActionResult Deactivate(string id)
    {
        var user = _appUserService.GetUser(id);

        user.LockoutEnabled = true;

        user.LockoutEnd = DateTime.Now.AddDays(1);

        _unitOfWork.Save();

        TempData["Danger"] = "User successfully deactivated";

        return RedirectToAction("Customer");
    }

    [HttpPost]
    public async Task<IActionResult> Password(UserPasswordViewModel model)
    {
        var appUser = _appUserService.GetUser(model.Id);

        var user = await _userManager.FindByIdAsync(appUser.Id);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

        if (result.Succeeded)
        {
            TempData["Success"] = "Password Successfully Updated";

            return RedirectToAction("Password");
        }

        TempData["Danger"] = result.Errors.FirstOrDefault().Description;

        return RedirectToAction("Password");
    }

    [HttpPost]
    public IActionResult Update(string id)
    {
        var user = _appUserService.GetUser(id);

        var customer = _customerService.GetAllCustomers().Where(x => x.UserId == user.Id).FirstOrDefault();

        if (customer.IsRegular)
        {
            customer.IsRegular = false;
            TempData["Success"] = "User's Status Changed to Normal";
        }
        else
        {
            customer.IsRegular = true;
            TempData["Success"] = "User's Status Changed to Regular";
        }
        _unitOfWork.Save();
        

        return RedirectToAction("Customer");
    }
        #endregion
}
