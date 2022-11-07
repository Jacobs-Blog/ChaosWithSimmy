using Api.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ProductsDatabase");
builder.Services.AddDatabase(connectionString);

var app = builder.Build();
await app.SeedDatabase(connectionString);
app.MapGet("/products/all", async (IProductsRepository products) =>
{
    var allProducts = await products.All();
    return Results.Json(allProducts);
});
app.Run();