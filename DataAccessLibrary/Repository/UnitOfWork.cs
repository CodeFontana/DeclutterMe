using DataAccessLibrary.Data;

namespace DataAccessLibrary.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly DeclutterMeDbContext _db;

    public UnitOfWork(DeclutterMeDbContext db)
    {
        _db = db;
    }

    public ICategoryRepository Category => new CategoryRepository(_db);
    public IProductRepository Product => new ProductRepository(_db);

    public async Task<bool> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync() > 0;
    }
}
