using DataAccessLibrary.Data;

namespace DataAccessLibrary.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly DeclutterMeDbContext _db;

    public CategoryRepository(DeclutterMeDbContext declutterMeDb) : base(declutterMeDb)
    {
        _db = declutterMeDb;
    }

    public async Task UpdateAsync(Category category)
    {
        Category dbCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

        if (dbCategory != null)
        {
            dbCategory.Name = category.Name;
            dbCategory.DisplayOrder = category.DisplayOrder;
        }
    }
}
