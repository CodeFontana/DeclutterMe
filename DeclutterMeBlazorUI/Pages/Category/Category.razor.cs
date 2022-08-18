namespace DeclutterMeBlazorUI.Pages.Category;

public partial class Category
{
	[Inject] DeclutterMeDbContext db { get; set; }

	private List<DataAccessLibrary.Entities.Category> _categories;

	protected override async Task OnInitializedAsync()
	{
        _categories = await db.Categories.AsNoTracking().ToListAsync();
    }
}
