using Api.Chaos;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace UnitTests.Chaos;

public class RepositoryBuilderShould
{
    private readonly ProductsDbContext _productsDbContext =
        new ProductsDbContext(new DbContextOptions<ProductsDbContext>());

    private readonly IOptions<ChaosSettings> _chaosSettings = Options.Create<ChaosSettings>(new()
    {
        ExceptionSettings = new()
        {
            Enabled = true,
            InjectionRate = 0.5
        },
        LatencySettings = new()
        {
            Enabled = true,
            InjectionRate = 0.5
        },
        ResultSettings = new()
        {
            Enabled = true,
            InjectionRate = 0.5
        },
        BehaviorSettings = new()
        {
            Enabled = true,
            InjectionRate = 0.5
        }
    });

    [Fact]
    public void AcceptRepository()
    {
        // ARRANGE
        var repository = new Mock<IProductsRepository>();

        // ACT
        var builder = new ProductsRepositoryBuilder(repository.Object, _chaosSettings, _productsDbContext);

        // ASSERT
        Assert.IsAssignableFrom<ProductsRepositoryBuilder>(builder);
    }

    [Fact]
    public void ReturnNormalRepositoryWhenNoOtherDecoratorIsEnabled()
    {
        // ARRANGE
        var repository = new Mock<IProductsRepository>();
        var builder = new ProductsRepositoryBuilder(repository.Object, _chaosSettings, _productsDbContext);

        // ACT
        var build = builder.Build();

        // ASSERT
        Assert.Equal(repository.Object, build);
    }

    [Fact]
    public void CreateRepositoryWithExceptionsDecorator()
    {
        // ARRANGE
        var repository = new Mock<IProductsRepository>();
        var builder = new ProductsRepositoryBuilder(repository.Object, _chaosSettings, _productsDbContext);

        // ACT
        var exceptionsRepository = builder.WithExceptions(true).Build();

        // ASSERT
        Assert.IsType<ExceptionsDecorator>(exceptionsRepository);
    }

    [Fact]
    public void CreateRepositoryWithLatencyDecorator()
    {
        // ARRANGE
        var repository = new Mock<IProductsRepository>();
        var builder = new ProductsRepositoryBuilder(repository.Object, _chaosSettings, _productsDbContext);

        // ACT
        var exceptionsRepository = builder.WithLatency(true).Build();

        // ASSERT
        Assert.IsType<LatencyDecorator>(exceptionsRepository);
    }

    [Fact]
    public void CreateRepositoryWithLatencyAndExceptionsDecorator()
    {
        // ARRANGE
        var repository = new Mock<IProductsRepository>();
        var builder = new ProductsRepositoryBuilder(repository.Object, _chaosSettings, _productsDbContext);

        // ACT
        var latencyRepository = builder.WithLatency(true).WithExceptions(true).Build();

        // ASSERT
        Assert.IsType<LatencyDecorator>(latencyRepository);
    }

    [Fact]
    public void CreateRepositoryWithResultsDecorator()
    {
        // ARRANGE
        var repository = new Mock<IProductsRepository>();
        var builder = new ProductsRepositoryBuilder(repository.Object, _chaosSettings, _productsDbContext);

        // ACT
        var exceptionsRepository = builder.WithResult(true).Build();

        // ASSERT
        Assert.IsType<ResultDecorator>(exceptionsRepository);
    }

    [Fact]
    public void CreateRepositoryWithBehaviorDecorator()
    {
        // ARRANGE
        var repository = new Mock<IProductsRepository>();
        var builder = new ProductsRepositoryBuilder(repository.Object, _chaosSettings, _productsDbContext);

        // ACT
        var exceptionsRepository = builder.WithBehavior(true).Build();

        // ASSERT
        Assert.IsType<BehaviorDecorator>(exceptionsRepository);
    }

    [Fact]
    public void CreateRepositoryWithAllDecorators()
    {
        // ARRANGE
        var repository = new Mock<IProductsRepository>();
        var builder = new ProductsRepositoryBuilder(repository.Object, _chaosSettings, _productsDbContext);

        // ACT
        var latencyRepository = builder
            .WithExceptions(true)
            .WithLatency(true)
            .WithResult(true)
            .WithBehavior(true)
            .Build();

        // ASSERT
        Assert.IsAssignableFrom<IProductsRepository>(latencyRepository);
    }
}