using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;
using VehicleRentalSystem.Domain.Entities;

namespace VehicleRentalSystem.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.Admin)]
public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailSender _emailSender;
    private readonly IStaffService _staffService;
    private readonly IFileTransferService _fileService;
    private readonly IAppUserService _appUserService;
    private readonly IRoleService _roleService;
    private readonly IUserRoleService _userRoleService;
    private readonly ICustomerService _customerService;

    public UserController(UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager, IEmailSender emailSender, 
        IStaffService staffService, IFileTransferService fileService, 
        IAppUserService appUserService, 
        IRoleService roleService, 
        IUserRoleService userRoleService,
        ICustomerService customerService)
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
    }

    #region Razor Views
    [HttpGet]
    public IActionResult Customer()
    {
        var users = _appUserService.GetAllUsers();
        var customers = _customerService.GetAllCustomers();

        var result = (from user in users
                      join customer in customers
                        on user.Id equals customer.UserId
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
    #endregion

    #region API Calls
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
    #endregion
}
