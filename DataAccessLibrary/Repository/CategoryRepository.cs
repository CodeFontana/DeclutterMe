using DataAccessLibrary.Data;

namespace DataAccessLibrary.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly DeclutterMeDbContext _db;

    public CategoryRepository(DeclutterMeDbContext declutterMeDb) : base(declutterMeDb)
    {
        _db = declutterMeDb;
    }

    public void Update(Category category)
    {
        _db.Categories.Update(category);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
