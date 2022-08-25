namespace DeclutterMeRazorUI.Pages.Categories;

[BindProperties]
public class EditModel : PageModel
{
    private readonly ICategoryRepository _db;

    public EditModel(ICategoryRepository db)
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
        if (Category.Name == Category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Category.Name", "The DisplayOrder cannot match the Name");
        }

        if (ModelState.IsValid)
        {
            _db.Update(Category);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category updated successfully";
            return RedirectToPage("Index");
        }
        
        return Page();
    }
}
