using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public static class DatabaseExtensions
{

   public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string connectionString)
   {
      serviceCollection.AddScoped<IProductsRepository, ProductsRepository>();
      serviceCollection.AddDbContext<ProductsDbContext>(options => options.UseNpgsql(connectionString));
      return serviceCollection;
   }
   
   public static async Task<WebApplication> SeedDatabase(this WebApplication app, string connectionString)
   {
      await using var scope = app.Services.CreateAsyncScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
      await dbContext.Database.EnsureDeletedAsync();
      await dbContext.Database.EnsureCreatedAsync();
      dbContext.Products.AddRange(FakeData.Products(40));
      await dbContext.SaveChangesAsync();
      return app;
   }
}