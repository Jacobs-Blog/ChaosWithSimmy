using Api.Data;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;

namespace Api.Chaos;

public class ResultDecorator : IProductsRepository
{
    private readonly IProductsRepository _inner;
    private readonly ResultSettings _resultSettings;
    private readonly AsyncInjectOutcomePolicy<HttpResponseMessage> _resultPolicy;
    private readonly Random _random;

    public ResultDecorator(IProductsRepository inner, ResultSettings resultSettings)
    {
        _inner = inner;
        _resultSettings = resultSettings;
        _random = new Random();
    }

    public async Task<List<Product>> All()
    {
        var products = await _inner.All();
        var resultPolicy = MonkeyPolicy.InjectResultAsync<List<Product>>(with =>
            with.Result(products.OrderBy(x => _random.Next()).Take(_random.Next(0, products.Count)).ToList())
                .InjectionRate(_resultSettings.InjectionRate)
                .Enabled());
        return await resultPolicy.ExecuteAsync(() => Task.FromResult(products));
    }

    public async Task<Product?> ById(int id)
    {
        var product = await _inner.ById(id);
        var resultPolicy = MonkeyPolicy.InjectResultAsync<Product>(with =>
            with.Result(new Product()
                {
                    Id = product.Id,
                    Name = $"{product.Name}, the Monkey Version",
                    Description= $"A monkey tampered with your product and changed the description and the price. The original price was ${product.Price}.",
                    Price = product.Price + 0.5m
                })
                .InjectionRate(_resultSettings.InjectionRate)
                .Enabled());
        return await resultPolicy.ExecuteAsync(() => Task.FromResult(product)); 
    }
}