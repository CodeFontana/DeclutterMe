namespace DeclutterMeRazorUI.Pages.Categories;

[BindProperties]
public class DeleteModel : PageModel
{
    private readonly ICategoryRepository _db;

    public DeleteModel(ICategoryRepository db)
    {
        _db = db;
    }

    public Category Category { get; set; }

    public async Task OnGet(int id)
    {
        Category = await _db.GetAsync(c => c.Id == id);
    }

    public async Task<IActionResult> OnPost()
    {
        _db.Remove(Category);
        await _db.SaveChangesAsync();
        TempData["success"] = "Category deleted successfully";
        return RedirectToPage("Index");
    }
}
