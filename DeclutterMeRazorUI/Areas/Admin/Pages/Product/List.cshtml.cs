namespace DeclutterMeRazorUI.Areas.Admin.Pages.Product;

public class ListModel : PageModel
{
    private readonly IUnitOfWork _db;

    public ListModel(IUnitOfWork db)
    {
        _db = db;
    }

    public IEnumerable<DataAccessLibrary.Entities.Product> Products { get; set; }

    public async Task<IActionResult> OnGet()
    {
        IEnumerable<DataAccessLibrary.Entities.Product> products = await _db.Product.GetWithCategoriesAsync();
        return new JsonResult(new { data = products });
    }
}
