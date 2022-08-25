namespace DataAccessLibrary.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
    Task SaveChangesAsync();
}
