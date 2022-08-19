namespace DeclutterMeRazorUI.Pages.Categories;

[BindProperties]
public class DeleteModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public DeleteModel(DeclutterMeDbContext db)
    {
        _db = db;
    }

    public Category Category { get; set; }

    public void OnGet(int id)
    {
        Category = _db.Categories.FirstOrDefault(c => c.Id == id);
    }

    public async Task<IActionResult> OnPost()
    {
        _db.Categories.Remove(Category);
        await _db.SaveChangesAsync();
        TempData["success"] = "Category deleted successfully";
        return RedirectToPage("Index");
    }
}
