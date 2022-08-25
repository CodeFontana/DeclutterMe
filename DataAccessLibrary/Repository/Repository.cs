using DataAccessLibrary.Data;

namespace DataAccessLibrary.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DeclutterMeDbContext _db;
    internal DbSet<T> dbSet;

    public Repository(DeclutterMeDbContext declutterMeDb)
    {
        _db = declutterMeDb;
        dbSet = _db.Set<T>();
    }
    
    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = dbSet
            .Where(filter);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAsync()
    {
        IQueryable<T> query = dbSet;
        return await query.ToListAsync();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void Remove(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }
}
