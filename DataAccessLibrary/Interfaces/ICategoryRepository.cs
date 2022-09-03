namespace DataAccessLibrary.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task UpdateAsync(Category category);
}
