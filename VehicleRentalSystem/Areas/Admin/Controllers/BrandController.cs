using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VehicleRentalSystem.Domain.Constants;
using VehicleRentalSystem.Application.Interfaces.Services;
using VehicleRentalSystem.Domain.Entities;
using System.Security.Claims;
using VehicleRentalSystem.Presentation.Areas.Admin.ViewModels;

namespace VehicleRentalSystem.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.Admin)]
public class BrandController : Controller
{
    private readonly IBrandService _brandService;
    private readonly IAppUserService _appUserService;
    private readonly IFileTransferService _fileService;

    public BrandController(IBrandService brandService,
        IAppUserService appUserService,
        IFileTransferService fileService)
    {
        _brandService = brandService;
        _appUserService = appUserService;
        _fileService = fileService;
    }

    #region Razor Views
    [HttpGet]
    public IActionResult Index()
    {
        var brands = _brandService.GetAllBrands();

        return View(brands);
    }

    [HttpGet]
    public IActionResult Details(Guid id)
    {
        var users = _appUserService.GetAllUsers();
        var brands = _brandService.GetAllBrands();

        var result = (from user in users
                      join brand in brands.Where(x => x.Id == id)
                        on user.Id equals brand.CreatedBy
                      join modifier in users
                        on brand.LastModifiedBy equals modifier.Id into modifierGroup
                      from modifier in modifierGroup.DefaultIfEmpty()
                      select new BrandViewModel
                      {
                          Id = brand.Id,
                          Name = brand.Name,
                          Description = brand.Description,
                          CreatedBy = user.FullName,
                          CreatedDate = brand.CreatedDate.ToString("dd/MM/yyyy"),
                          Image = brand.Image,
                          IsActive = brand.IsActive,
                          LastModifiedBy = modifier != null ? modifier.FullName : "Not modified yet",
                          LastModifiedDate = modifier != null ? brand.LastModifiedDate?.ToString("dd/MM/yyyy") : "Not modified yet",
                      }).FirstOrDefault();

        return View(result);
    }

    [HttpGet]
    public IActionResult Upsert(Guid id)
    {
        var brand = new Brand();

        if (id == Guid.Empty)
        {
            return View(brand);
        }

        brand = _brandService.GetBrand(id);

        if (brand != null)
        {
            return View(brand);
        }

        return NotFound();
    }

    [HttpGet]
    public IActionResult Delete(Guid id)
    {
        var brand = _brandService.GetBrand(id);

        if (brand != null)
        {
            return View(brand);
        }

        return NotFound();
    }
    #endregion

    #region API Calls
    [HttpPost, ActionName("Upsert")]
    public IActionResult UpsertBrand(Brand brand)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;

        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        var image = Request.Form.Files.FirstOrDefault();

        if (brand.Id == Guid.Empty)
        {
            var item = new Brand()
            {
                Image = _fileService.ImageByte(image),
                ImageURL = _fileService.FilePath(image, Constants.Brand, brand.Name, ""),
                Name = brand.Name,
                Description = brand.Description.Replace("<p>", "").Replace("</p>", ""),
                CreatedBy = claim.Value,
            };

            _brandService.AddBrand(item);

            TempData["Success"] = "Brand successfully added";
        }
        else
        {
            var item = new Brand()
            {
                Id = brand.Id,
                Image = _fileService.ImageByte(image),
                ImageURL = _fileService.FilePath(image, Constants.Brand, brand.Name, ""),
                Name = brand.Name,
                Description = brand.Description.Replace("<p>", "").Replace("</p>", ""),
                LastModifiedBy = claim.Value,
            };

            _brandService.UpdateBrand(item);

            TempData["Info"] = "Brand successfully updated";
        }

        return RedirectToAction("Index");
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteBrand(Guid id)
    {
        var brand = _brandService.GetBrand(id);

        if (brand != null)
        {
            _brandService.DeleteBrand(brand.Id);

            TempData["Delete"] = "Brand successfully deleted";

            return RedirectToAction("Index");

        }

        return NotFound();
    }
    #endregion
}
