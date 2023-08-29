using Api.Data;
using Polly;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Latency;
using Polly.Contrib.Simmy.Outcomes;
using Polly.Wrap;

namespace Api.Chaos;

public class RandomPolicyDecorator : IProductsRepository
{
    private readonly IProductsRepository _inner;
    private readonly AsyncPolicyWrap _wrappedPolicies;

    public RandomPolicyDecorator(IProductsRepository inner, ChaosSettings chaosSettings)
    {
        _inner = inner;

        var shortLatency = MonkeyPolicy.InjectLatencyAsync(with =>
                with.Latency(TimeSpan.FromMilliseconds(chaosSettings.LatencySettings.MsLatency))
                    .InjectionRate(chaosSettings.LatencySettings.InjectionRate)
                    .Enabled());

        var longLatency = MonkeyPolicy.InjectLatencyAsync(with =>
                with.Latency(TimeSpan.FromSeconds(chaosSettings.LatencySettings.SecondsLatency))
                    .InjectionRate(chaosSettings.LatencySettings.InjectionRate)
                    .Enabled());
        
        var exception = MonkeyPolicy.InjectExceptionAsync((with) =>
            with.Fault(new InvalidDataException("Chaos Monkey says Hi!"))
                .InjectionRate(chaosSettings.ExceptionSettings.InjectionRate)
                .Enabled());

        _wrappedPolicies = Policy.WrapAsync(shortLatency, longLatency, exception);
    }

    public async Task<List<Product>> All() => 
        await _wrappedPolicies.ExecuteAsync(async () => await _inner.All());

    public async Task<Product?> ById(int id) =>
        await _wrappedPolicies.ExecuteAsync(async () => await _inner.ById(id));
}