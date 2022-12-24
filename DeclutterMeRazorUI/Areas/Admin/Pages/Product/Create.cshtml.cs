using DataAccessLibrary.Data;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeRazorUI.Areas.Admin.Pages.Product;

public class CreateModel : PageModel
{
    private readonly DeclutterMeDbContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public CreateModel(DeclutterMeDbContext db, IWebHostEnvironment hostEnvironment)
    {
        _db = db;
        _hostEnvironment = hostEnvironment;
    }

    public IEnumerable<SelectListItem> CategoryList { get; set; }

    [BindProperty]
    public DataAccessLibrary.Entities.Product Product { get; set; }

    public async Task OnGetAsync()
    {
        Product = new();
        CategoryList = (await _db.Categories.ToListAsync())
            .Select(c => new SelectListItem(c.Name, c.Id.ToString()));
    }

    public async Task<IActionResult> OnPostAsync(IFormFile file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (file is not null)
            {
                string fileName = $"{Product.Name}-{Guid.NewGuid()}";
                string uploads = Path.Combine(wwwRootPath, @"img\products");
                string extension = Path.GetExtension(file.FileName);

                Directory.CreateDirectory(uploads);

                if (Product.ImageUrl != null)
                {
                    string oldImagePath = Path.Combine(wwwRootPath, Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create);
                file.CopyTo(fileStream);
                Product.ImageUrl = $@"\img\products\{fileName}{extension}";
            }

            await _db.Products.AddAsync(Product);
            TempData["success"] = "Product created successfully";
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        return Page();
    }
}
