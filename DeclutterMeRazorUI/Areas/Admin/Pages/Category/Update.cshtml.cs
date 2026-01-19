using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeRazorUI.Areas.Admin.Pages.Category;

[BindProperties]
public class UpdateModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public UpdateModel(DeclutterMeDbContext db)
    {
        _db = db;
    }

    public DataAccessLibrary.Entities.Category Category { get; set; }

    public async Task OnGet(int id)
    {
        Category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
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
