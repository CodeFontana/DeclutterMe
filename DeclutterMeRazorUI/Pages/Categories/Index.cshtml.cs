using DataAccessLibrary.Entities;

namespace DeclutterMeRazorUI.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public IndexModel(DeclutterMeDbContext db)
    {
        _db = db;
    }

    public List<Category> Categories { get; set; }

    public async Task OnGet()
    {
        Categories = await _db.Categories.AsNoTracking().ToListAsync();
    }
}
