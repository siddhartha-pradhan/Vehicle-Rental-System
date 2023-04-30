using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Domain.Entities;
using VehicleRentalSystem.Presentation.Areas.Account.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.Account.Controllers;

[Authorize]
[Area("Account")]
public class ProfileController : Controller
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly IAppUserService _appUserService;
	private readonly IWebHostEnvironment _webHostEnvironment;
	private readonly IEmailSender _emailSender;
	private readonly IFileTransferService _fileService;

	public ProfileController(UserManager<IdentityUser> userManager,
		SignInManager<IdentityUser> signInManager,
		IAppUserService appUserService,
		IWebHostEnvironment webHostEnvironment,
		IEmailSender emailSender,
		IFileTransferService fileService)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_appUserService = appUserService;
		_webHostEnvironment = webHostEnvironment;
		_emailSender = emailSender;
		_fileService = fileService;
	}

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

	public IActionResult Password()
	{
		var passwordViewModel = new PasswordViewModel();

		return View(passwordViewModel);
	}

	[HttpPost]
	public async Task<IActionResult> Profile(ProfileViewModel profile)
	{
		var user = await _userManager.GetUserAsync(User);
		var roles = await _userManager.GetRolesAsync(user);
		var role = roles.ToList().FirstOrDefault();
		var appUser = _appUserService.GetUser(user.Id);
		var file = Request.Form.Files.FirstOrDefault();

		appUser.Image = _fileService.ImageByte(file);
		appUser.ImageURL = _fileService.FilePath(file, Constants.User, appUser.FullName, role);

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
}