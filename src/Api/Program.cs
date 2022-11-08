using Api;
using Api.Chaos;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("ProductsDatabase");
builder.Services.Configure<ChaosSettings>(builder.Configuration.GetSection(nameof(ChaosSettings)));
builder.Services.AddDatabase(connectionString);
builder.Services.AddScoped<ProductsRepositoryBuilder>();

var app = builder.Build();
await app.SeedDatabase(connectionString);
app.AddRoutes();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();