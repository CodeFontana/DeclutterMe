namespace DeclutterMeMvcUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _db;

    public CategoryController(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Category> categories = await _db.Category.GetAsync();
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
            await _db.Category.AddAsync(category);
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

        Category category = await _db.Category.GetAsync(c => c.Id == id);

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
            await _db.Category.UpdateAsync(category);
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

        Category category = await _db.Category.GetAsync(c => c.Id == id);

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
        Category c = await _db.Category.GetAsync(c => c.Id == category.Id);

        if (c == null)
        {
            return NotFound();
        }

        _db.Category.Remove(c);
        await _db.SaveChangesAsync();
        TempData["success"] = "Category deleted successfully";

        return RedirectToAction("Index");
    }
}
