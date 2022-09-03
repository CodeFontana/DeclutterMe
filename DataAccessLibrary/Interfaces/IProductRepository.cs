namespace DataAccessLibrary.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task UpdateAsync(Product product);
}
