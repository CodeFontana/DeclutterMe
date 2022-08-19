namespace DeclutterMeRazorUI.Pages.Categories;

[BindProperties]
public class CreateModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public CreateModel(DeclutterMeDbContext db)
    {
        _db = db;
    }

    public Category Category { get; set; }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPost()
    {
        if (Category.Name == Category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Category.Name", "The DisplayOrder cannot match the Name");
        }

        if (ModelState.IsValid)
        {
            await _db.Categories.AddAsync(Category);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category created successfully";
            return RedirectToPage("Index");
        }
        
        return Page();
    }
}
