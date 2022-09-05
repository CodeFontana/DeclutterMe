namespace DeclutterMeRazorUI.Areas.Admin.Pages.Product;

public class DeleteModel : PageModel
{
    private readonly IUnitOfWork _db;

    public DeleteModel(IUnitOfWork db)
    {
        _db = db;
    }

    [BindProperty]
    public DataAccessLibrary.Entities.Product Product { get; set; }

    public async Task OnGet(int id)
    {
        Product = await _db.Product.GetWithCategoryAsync(id);
    }

    public async Task<IActionResult> OnPost()
    {
        _db.Product.Remove(Product);
        await _db.SaveChangesAsync();
        TempData["success"] = "Product deleted successfully";
        return RedirectToPage("Index");
    }
}
