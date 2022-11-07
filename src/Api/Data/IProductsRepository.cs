namespace Api.Data;

public interface IProductsRepository
{
    Task<List<Product>> All();

    Task<Product?> ById(int id);
}