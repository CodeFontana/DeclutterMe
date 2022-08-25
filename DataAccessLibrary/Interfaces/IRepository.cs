namespace DataAccessLibrary.Interfaces;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T> GetAsync(Expression<Func<T, bool>> filter);
    Task<IEnumerable<T>> GetAsync();
    void Remove(T entity);
    void Remove(IEnumerable<T> entity);
}
