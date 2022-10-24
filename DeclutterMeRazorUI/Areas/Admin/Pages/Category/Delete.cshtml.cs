using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeRazorUI.Areas.Admin.Pages.Category;

[BindProperties]
public class DeleteModel : PageModel
{
    private readonly DeclutterMeDbContext _db;

    public DeleteModel(DeclutterMeDbContext db)
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
        _db.Categories.Remove(Category);
        await _db.SaveChangesAsync();
        TempData["success"] = "Category deleted successfully";
        return RedirectToPage("Index");
    }
}
