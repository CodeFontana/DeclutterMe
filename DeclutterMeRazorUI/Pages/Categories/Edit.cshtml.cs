namespace DeclutterMeRazorUI.Pages.Categories;

[BindProperties]
public class EditModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public EditModel(DeclutterMeDbContext db)
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
        if (Category.Name == Category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Category.Name", "The DisplayOrder cannot match the Name");
        }

        if (ModelState.IsValid)
        {
            _db.Categories.Update(Category);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category updated successfully";
            return RedirectToPage("Index");
        }
        
        return Page();
    }
}
