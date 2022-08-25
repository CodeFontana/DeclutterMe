namespace DeclutterMeRazorUI.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly ICategoryRepository _db;

    public IndexModel(ICategoryRepository db)
    {
        _db = db;
    }

    public IEnumerable<Category> Categories { get; set; }

    public async Task OnGet()
    {
        Categories = await _db.GetAsync();
    }
}
