using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeRazorUI.Areas.Admin.Pages.Product;

public class EditModel : PageModel
{
    private readonly DeclutterMeDbContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public EditModel(DeclutterMeDbContext db, IWebHostEnvironment hostEnvironment)
    {
        _db = db;
        _hostEnvironment = hostEnvironment;
    }

    public IEnumerable<SelectListItem> CategoryList { get; set; }

    [BindProperty]
    public DataAccessLibrary.Entities.Product Product { get; set; }

    public async Task OnGet(int id)
    {
        Product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
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

            _db.Products.Update(Product);
            TempData["success"] = "Product updated successfully";
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        return Page();
    }
}
