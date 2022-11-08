using System.Diagnostics;
using Api.Chaos;
using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api;

public static class Routes
{
    public static WebApplication AddRoutes(this WebApplication app)
    {
        app.MapGet("/products/all", async (
            [FromServices] ProductsRepositoryBuilder productsRepositoryBuilder,
            [FromServices] IOptions<ChaosSettings> chaosSettings) =>
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            var products = productsRepositoryBuilder
                .WithExceptions(chaosSettings.Value.ExceptionSettings.Enabled)
                .WithLatency(chaosSettings.Value.LatencySettings.Enabled)
                .WithResult(chaosSettings.Value.ResultSettings.Enabled)
                .WithBehavior(chaosSettings.Value.BehaviorSettings.Enabled)
                .Build();

            var allProducts = await products.All();
            stopwatch.Stop();

            return Results.Json(new ProductsResult
            {
                RequestDuration = stopwatch.ElapsedMilliseconds,
                ProductsList = allProducts
            });
        });

        app.MapGet("/products/{productId}", async (
            [FromServices] ProductsRepositoryBuilder productsRepositoryBuilder,
            [FromServices] IOptions<ChaosSettings> chaosSettings,
            int productId) =>
        {
            var products = productsRepositoryBuilder
                .WithExceptions(chaosSettings.Value.ExceptionSettings.Enabled)
                .WithLatency(chaosSettings.Value.LatencySettings.Enabled)
                .WithResult(chaosSettings.Value.ResultSettings.Enabled)
                .WithBehavior(chaosSettings.Value.BehaviorSettings.Enabled)
                .Build();

            Stopwatch stopwatch = new();
            stopwatch.Start();
            var product = await products.ById(productId);
            stopwatch.Stop();

            return Results.Json(new ProductsResult
            {
                RequestDuration = stopwatch.ElapsedMilliseconds,
                ProductsList = new List<Product> {product}
            });
        });

        app.MapPost("/fix-database", async ([FromServices] ProductsDbContext dbContext) =>
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Products.AddRange(FakeData.Products(40));
            await dbContext.SaveChangesAsync();
            return Results.Accepted();
        });
        return app;
    }
}