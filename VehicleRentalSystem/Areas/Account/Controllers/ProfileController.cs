using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Constants;
using Microsoft.AspNetCore.Identity.UI.Services;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Presentation.Areas.Account.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.Account.Controllers;

[Authorize]
[Area("Account")]
public class ProfileController : Controller
{
    #region Service Injection
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IAppUserService _appUserService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IEmailSender _emailSender;
    private readonly ICustomerService _customerService;
    private readonly IFileTransferService _fileService;

    public ProfileController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IAppUserService appUserService,
        IWebHostEnvironment webHostEnvironment,
        IEmailSender emailSender,
        ICustomerService customerService,
        IFileTransferService fileService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _appUserService = appUserService;
        _webHostEnvironment = webHostEnvironment;
        _customerService = customerService;
        _emailSender = emailSender;
        _fileService = fileService;
    }
    #endregion

    #region Razor Views
    /// <summary>
    /// Defining a method to view the logged in user's profile
    /// </summary>
    [HttpGet]
    public IActionResult Profile()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var user = _appUserService.GetUser(userId);
        var userName = user.FullName;
        var phoneNumber = user.PhoneNumber;
        var appUser = _appUserService.GetUser(user.Id);
        var image = appUser.Image;

        var profile = new ProfileViewModel()
        {
            Image = image,
            PhoneNumber = phoneNumber,
            Username = userName,
        };

        return View(profile);
    }

    /// <summary>
    /// Defining a view to display the logged in user's documents page
    /// </summary>
    [HttpGet]
    public IActionResult Documents()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var user = _appUserService.GetUser(userId);
        var customer = _customerService.GetUser(userId);

        var details = new DetailViewModel()
        {
            CitizenshipNumber = customer.CitizenshipNumber,
            LicenseNumber = customer.LicenseNumber,
            ExpirationDate = customer.ExpirationDate,
            CitizenshipURL = customer.CitizenshipURL,
            LicenseURL = customer.LicenseURL
        };

        return View(details);

    }

    /// <summary>
    /// Defining a view for users to be able to change their passwords
    /// </summary>
    [HttpGet]
    public IActionResult Password()
    {
        var passwordViewModel = new PasswordViewModel();

        return View(passwordViewModel);
    }
    #endregion

    #region API Calls
    /// <summary>
    /// Defining a post action for users to be able to update their profiles
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Profile(ProfileViewModel profile)
    {
        var user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.ToList().FirstOrDefault();
        var appUser = _appUserService.GetUser(user.Id);
        var file = Request.Form.Files.FirstOrDefault();

        if (file != null)
        {
            appUser.Image = _fileService.ImageByte(file);
            appUser.ImageURL = _fileService.FilePath(file, Constants.User, appUser.FullName, role);
        }

        await _userManager.UpdateAsync(user);

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        if (profile.PhoneNumber != phoneNumber)
        {
            await _userManager.SetPhoneNumberAsync(user, profile.PhoneNumber);
        }

        await _signInManager.RefreshSignInAsync(user);

        TempData["Success"] = "Profile Updated Successfully";

        return RedirectToAction("Profile");
    }

    /// <summary>
    /// Defining a post action for users to be able to change their profile passwords
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Password(PasswordViewModel passwordViewModel)
    {
        var user = await _userManager.GetUserAsync(User);
        var appUser = _appUserService.GetUser(user.Id);

        await _userManager.ChangePasswordAsync(user, passwordViewModel.OldPassword, passwordViewModel.NewPassword);

        await _signInManager.RefreshSignInAsync(user);

        await _emailSender.SendEmailAsync(user.Email, "Password Change",
                    $"Dear {appUser.FullName},<br><br>Your password has been changed in our system. " +
                    $"<br>Please visit the store if it was not your action." +
                    $"<br><br>Regards,<br>Hajur ko Car Rental");

        TempData["Success"] = "Password Successfully Updated";

        return RedirectToAction("Password");

    }

    /// <summary>
    /// Defining a post action for users to be able to upload their documents in the form of form file of license and citizenship
    /// </summary>
    [HttpPost]
    public IActionResult Documents(DetailViewModel model, IFormFile license, IFormFile citizenship)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        var user = _appUserService.GetUser(userId);
        var customer = _customerService.GetUser(userId);

        customer.CitizenshipNumber = model.CitizenshipNumber;
        customer.LicenseNumber = model.LicenseNumber;
        customer.ExpirationDate = model.ExpirationDate;

        if(license != null)
        {
            if(license.Length > 1572864)
            {
                TempData["Danger"] = "Please enter a file of size less than 1.5MB.";
                return RedirectToAction("Documents");
            }

            customer.LicenseURL = _fileService.FilePath(license, Constants.Licenses.ToLower(), user.FullName, "");
        }
        if(citizenship != null)
        {
            if (citizenship.Length > 1572864)
            {
                TempData["Danger"] = "Please enter a file of size less than 1.5MB.";
                return RedirectToAction("Documents");
            }

            customer.CitizenshipURL = _fileService.FilePath(citizenship, Constants.Citizenship.ToLower(), user.FullName, "");
        }

        _customerService.UpdateCustomer(customer);

        TempData["Success"] = "Documents Updated Successfully";

        return RedirectToAction("Profile");
    }
    #endregion
}