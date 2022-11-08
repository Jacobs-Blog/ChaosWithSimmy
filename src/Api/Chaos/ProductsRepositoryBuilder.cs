using Api.Data;
using Microsoft.Extensions.Options;

namespace Api.Chaos;

public class ProductsRepositoryBuilder
{
    private readonly IProductsRepository _productsRepository;
    private readonly ProductsDbContext _productsDbContext;
    private readonly ChaosSettings _chaosSettings;
    private bool _exceptions;
    private bool _latency;
    private bool _result;
    private bool _behavior;

    public ProductsRepositoryBuilder(IProductsRepository productsRepository, IOptions<ChaosSettings> chaosSettings, ProductsDbContext productsDbContext)
    {
        _productsRepository = productsRepository;
        _productsDbContext = productsDbContext;
        _chaosSettings = chaosSettings.Value;
    }

    public ProductsRepositoryBuilder WithExceptions(bool isEnabled)
    {
        _exceptions = isEnabled;
        return this;
    }

    public ProductsRepositoryBuilder WithLatency(bool isEnabled)
    {
        _latency = isEnabled;
        return this;
    }

    public ProductsRepositoryBuilder WithResult(bool isEnabled)
    {
        _result = isEnabled;
        return this;
    }

    public ProductsRepositoryBuilder WithBehavior(bool isEnabled)
    {
        _behavior = isEnabled;
        return this;
    }

    public IProductsRepository Build()
    {
        IProductsRepository build = null;
        if (_exceptions)
            build = new ExceptionsDecorator(_productsRepository, _chaosSettings.ExceptionSettings);

        if (_latency)
            build = build == null 
                ? new LatencyDecorator(_productsRepository, _chaosSettings.LatencySettings) 
                : new LatencyDecorator(build, _chaosSettings.LatencySettings);

        if (_result)
            build = build == null
                ? new ResultDecorator(_productsRepository, _chaosSettings.ResultSettings)
                : new ResultDecorator(build, _chaosSettings.ResultSettings);

        if (_behavior)
            build = build == null
                ? new BehaviorDecorator(_productsRepository, _chaosSettings.BehaviorSettings, _productsDbContext)
                : new BehaviorDecorator(build, _chaosSettings.BehaviorSettings, _productsDbContext);

        return build ?? _productsRepository;
    }
}