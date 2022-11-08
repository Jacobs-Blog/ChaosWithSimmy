using Api.Data;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;

namespace Api.Chaos;

public class ExceptionsDecorator : IProductsRepository
{
    private readonly IProductsRepository _inner;
    private readonly AsyncInjectOutcomePolicy _exceptionPolicy;

    public ExceptionsDecorator(IProductsRepository inner, ExceptionSettings exceptionSettings)
    {
        _inner = inner;
        _exceptionPolicy = MonkeyPolicy
            .InjectExceptionAsync((with) =>
                with.Fault(new InvalidDataException("Chaos Monkey says Hi!"))
                    .InjectionRate(exceptionSettings.InjectionRate)
                    .Enabled());
    }

    public async Task<List<Product>> All() =>
        await _exceptionPolicy.ExecuteAsync(async () => await _inner.All());

    public async Task<Product?> ById(int id) =>
        await _exceptionPolicy.ExecuteAsync(async () => await _inner.ById(id));
}