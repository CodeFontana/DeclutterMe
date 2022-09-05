namespace DeclutterMeRazorUI.Areas.Admin.Pages.Category;

public class IndexModel : PageModel
{
    private readonly IUnitOfWork _db;

    public IndexModel(IUnitOfWork db)
    {
        _db = db;
    }

    public IEnumerable<DataAccessLibrary.Entities.Category> Categories { get; set; }

    public async Task OnGet()
    {
        Categories = await _db.Category.GetAsync();
    }
}