using Api.Data;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Behavior;

namespace Api.Chaos;

public class BehaviorDecorator : IProductsRepository
{
    private readonly IProductsRepository _inner;
    private readonly AsyncInjectBehaviourPolicy _behaviorPolicy;

    public BehaviorDecorator(IProductsRepository inner, BehaviorSettings behaviorSettings, ProductsDbContext productsDbContext)
    {
        _inner = inner;
        _behaviorPolicy = MonkeyPolicy.InjectBehaviourAsync(with =>
            with.Behaviour(async () => await productsDbContext.Database.EnsureDeletedAsync())
                .InjectionRate(behaviorSettings.InjectionRate)
                .Enabled());
    }

    public async Task<List<Product>> All() =>
        await _behaviorPolicy.ExecuteAsync(async () => await _inner.All());

    public async Task<Product?> ById(int id) =>
        await _behaviorPolicy.ExecuteAsync(async () => await _inner.ById(id));
}