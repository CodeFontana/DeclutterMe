namespace DataAccessLibrary.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetWithCategoriesAsync();
    Task<Product> GetWithCategoryAsync(int id);
    Task UpdateAsync(Product product);
}
