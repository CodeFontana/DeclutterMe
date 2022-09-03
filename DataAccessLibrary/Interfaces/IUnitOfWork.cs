namespace DataAccessLibrary.Interfaces;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    Task<bool> SaveChangesAsync();
}
