using DataAccessLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Data;

public class DeclutterMeDbContext : DbContext
{
	public DeclutterMeDbContext(DbContextOptions<DeclutterMeDbContext> options) : base(options)
	{

	}

	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
}
