using Api.Data;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Latency;

namespace Api.Chaos;

public class LatencyDecorator : IProductsRepository
{
    private readonly IProductsRepository _inner;
    private readonly AsyncInjectLatencyPolicy _latencyPolicy;

    public LatencyDecorator(IProductsRepository inner, LatencySettings latencySettings)
    {
        _inner = inner;
        _latencyPolicy = MonkeyPolicy
            .InjectLatencyAsync(with =>
                with.Latency(TimeSpan.FromMilliseconds(latencySettings.MsLatency))
                    .InjectionRate(latencySettings.InjectionRate)
                    .Enabled());
    }

    public async Task<List<Product>> All() => 
        await _latencyPolicy.ExecuteAsync(async () => await _inner.All());

    public async Task<Product?> ById(int id) =>
        await _latencyPolicy.ExecuteAsync(async () => await _inner.ById(id));
}