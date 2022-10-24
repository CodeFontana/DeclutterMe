using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeRazorUI.Areas.Admin.Pages.Product;

public class DeleteModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public DeleteModel(DeclutterMeDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public DataAccessLibrary.Entities.Product Product { get; set; }

    public async Task OnGet(int id)
    {
        Product = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IActionResult> OnPost()
    {
        _db.Products.Remove(Product);
        await _db.SaveChangesAsync();
        TempData["success"] = "Product deleted successfully";
        return RedirectToPage("Index");
    }
}
