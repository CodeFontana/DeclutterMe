using DataAccessLibrary.Entities;

namespace DeclutterMeRazorUI.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public IndexModel(DeclutterMeDbContext db)
    {
        _db = db;
    }

    public IEnumerable<Category> Categories { get; set; }

    public void OnGet()
    {
        Categories = _db.Categories.AsNoTracking();
    }
}
