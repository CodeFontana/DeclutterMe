namespace DeclutterMeRazorUI.Areas.Admin.Pages.Category;

[BindProperties]
public class DeleteModel : PageModel
{
    private readonly IUnitOfWork _db;

    public DeleteModel(IUnitOfWork db)
    {
        _db = db;
    }

    public DataAccessLibrary.Entities.Category Category { get; set; }

    public async Task OnGet(int id)
    {
        Category = await _db.Category.GetAsync(c => c.Id == id);
    }

    public async Task<IActionResult> OnPost()
    {
        _db.Category.Remove(Category);
        await _db.SaveChangesAsync();
        TempData["success"] = "Category deleted successfully";
        return RedirectToPage("Index");
    }
}
