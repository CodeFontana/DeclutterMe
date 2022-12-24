using DataAccessLibrary.Data;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeMvcUI.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly DeclutterMeDbContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(DeclutterMeDbContext db, IWebHostEnvironment hostEnvironment)
    {
        _db = db;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IEnumerable<Product> products = await _db.Products
            .Include(p => p.Category)
            .ToListAsync();
        return Json(new { data = products });
    }

    public async Task<IActionResult> Upsert(int? id)
    {
        ProductUpsertModel productUpsertModel = new()
        {
            Product = new(),
            CategoryList = (await _db.Categories.ToListAsync())
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
        };

        if (id is null || id == 0)
        {
            return View(productUpsertModel);
        }
        else
        {
            productUpsertModel.Product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            return View(productUpsertModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpsertAsync(ProductUpsertModel productUpdate, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (file is not null)
            {
                string fileName = $"{productUpdate.Product.Name}-{Guid.NewGuid()}";
                string uploads = Path.Combine(wwwRootPath, @"img\products");
                string extension = Path.GetExtension(file.FileName);

                Directory.CreateDirectory(uploads);

                if (productUpdate.Product.ImageUrl != null)
                {
                    string oldImagePath = Path.Combine(wwwRootPath, productUpdate.Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create);
                file.CopyTo(fileStream);
                productUpdate.Product.ImageUrl = $@"\img\products\{fileName}{extension}";
            }

            if (productUpdate.Product.Id == 0)
            {
                await _db.Products.AddAsync(productUpdate.Product);
                TempData["success"] = "Product created successfully";
            }
            else
            {
                _db.Products.Update(productUpdate.Product);
                TempData["success"] = "Product updated successfully";
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(productUpdate);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int? id)
    {
        Product dbProduct = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (dbProduct == null)
        {
            return Json(new { success = false, message = $"Product not found in database [{id}]"});
        }

        string oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, dbProduct.ImageUrl.TrimStart('\\'));

        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _db.Products.Remove(dbProduct);
        await _db.SaveChangesAsync();
        return Json(new { success = true, message = $"Product deleted successfully" });
    }
}
