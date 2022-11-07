using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products => Set<Product>();
}