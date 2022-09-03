namespace DataAccessLibrary.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetWithCategoriesAsync();
    Task UpdateAsync(Product product);
}
