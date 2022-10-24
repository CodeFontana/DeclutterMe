using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeRazorUI.Areas.Admin.Pages.Product;

public class ListModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public ListModel(DeclutterMeDbContext db)
    {
        _db = db;
    }

    public IEnumerable<DataAccessLibrary.Entities.Product> Products { get; set; }

    public async Task<IActionResult> OnGet()
    {
        IEnumerable<DataAccessLibrary.Entities.Product> products = await _db.Products
            .Include(p => p.Category)
            .ToListAsync();

        return new JsonResult(new { data = products });
    }
}
