using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeclutterMeRazorUI.Areas.Admin.Pages.Product;

public class CreateModel : PageModel
{
    private readonly IUnitOfWork _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public CreateModel(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
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
        CategoryList = (await _db.Category.GetAsync()).Select(c => new SelectListItem(c.Name, c.Id.ToString()));
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

            await _db.Product.AddAsync(Product);
            TempData["success"] = "Product created successfully";
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        return Page();
    }
}