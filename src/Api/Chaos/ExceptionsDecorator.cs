using Api.Data;

namespace Api.Chaos;

public class ExceptionsDecorator : IProductsRepository
{
    public Task<List<Product>> All()
    {
        throw new NotImplementedException();
    }

    public Task<Product?> ById(int id)
    {
        throw new NotImplementedException();
    }
}