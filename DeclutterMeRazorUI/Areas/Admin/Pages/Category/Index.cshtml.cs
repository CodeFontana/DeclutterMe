using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeRazorUI.Areas.Admin.Pages.Category;

public class IndexModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public IndexModel(DeclutterMeDbContext db)
    {
        _db = db;
    }

    public IEnumerable<DataAccessLibrary.Entities.Category> Categories { get; set; }

    public async Task OnGet()
    {
        Categories = await _db.Categories.ToListAsync();
    }
}
