namespace DeclutterMeMvcUI.Controllers;

public class CategoryController : Controller
{
    private readonly DeclutterMeDbContext _db;

    public CategoryController(DeclutterMeDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        List<Category> categories = await _db.Categories.AsNoTracking().ToListAsync();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The DisplayOrder cannot match the Name");
        }

        if (ModelState.IsValid)
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }

        return View(category);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category? category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The DisplayOrder cannot match the Name");
        }

        if (ModelState.IsValid)
        {
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }

        return View(category);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category? category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Category category)
    {
        Category? c = await _db.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

        if (c == null)
        {
            return NotFound();
        }

        _db.Categories.Remove(c);
        await _db.SaveChangesAsync();
        TempData["success"] = "Category deleted successfully";

        return RedirectToAction("Index");
    }
}
