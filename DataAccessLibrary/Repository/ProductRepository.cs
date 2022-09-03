namespace DataAccessLibrary.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly DeclutterMeDbContext _db;

    public ProductRepository(DeclutterMeDbContext declutterMeDb) : base(declutterMeDb)
    {
        _db = declutterMeDb;
    }

    public async Task<IEnumerable<Product>> GetWithCategoriesAsync()
    {
        return await _db.Products
            .Include(p => p.Category)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        Product dbProduct = await _db.Products.FirstOrDefaultAsync(u => u.Id == product.Id);

        if (dbProduct != null)
        {
            dbProduct.Name = product.Name;
            dbProduct.Description = product.Description;
            dbProduct.ListPrice = product.ListPrice;
            dbProduct.ActualPrice = product.ActualPrice;
            dbProduct.CategoryId = product.CategoryId;

            if (product.ImageUrl != null)
            {
                dbProduct.ImageUrl = product.ImageUrl;
            }
        }
    }
}
