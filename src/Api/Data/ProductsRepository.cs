using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class ProductsRepository : IProductsRepository
{
    private readonly ProductsDbContext _productsDbContext;

    public ProductsRepository(ProductsDbContext productsDbContext)
    {
        _productsDbContext = productsDbContext;
    }

    public async Task<List<Product>> All() => await _productsDbContext.Products.ToListAsync();

    public async Task<Product?> ById(int id) =>
        await _productsDbContext.Products.FirstOrDefaultAsync(with => with.Id == id);
}