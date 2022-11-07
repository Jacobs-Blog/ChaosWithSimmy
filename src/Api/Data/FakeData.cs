using Bogus;

namespace Api.Data;

public static class FakeData
{
    public static List<Product> Products(int amount) => new Faker<Product>()
        .RuleFor(product => product.Name, f => f.Commerce.ProductName())
        .RuleFor(product => product.Description, f => f.Commerce.ProductDescription())
        .RuleFor(product => product.Price, f => decimal.Parse(f.Commerce.Price()))
        .Generate(amount);
}