namespace DataAccessLibrary.Interfaces;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    Task<bool> SaveChangesAsync();
}
