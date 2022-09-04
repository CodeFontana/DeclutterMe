namespace DeclutterMeRazorUI.Areas.Admin.Pages.Categories;

[BindProperties]
public class EditModel : PageModel
{
    private readonly IUnitOfWork _db;

    public EditModel(IUnitOfWork db)
    {
        _db = db;
    }

    public Category Category { get; set; }

    public async Task OnGet(int id)
    {
        Category = await _db.Category.GetAsync(c => c.Id == id);
    }

    public async Task<IActionResult> OnPost()
    {
        if (Category.Name == Category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Category.Name", "The DisplayOrder cannot match the Name");
        }

        if (ModelState.IsValid)
        {
            await _db.Category.UpdateAsync(Category);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category updated successfully";
            return RedirectToPage("Index");
        }
        
        return Page();
    }
}
