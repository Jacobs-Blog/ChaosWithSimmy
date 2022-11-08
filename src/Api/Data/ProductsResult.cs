namespace Api.Data;

public class ProductsResult
{
    public long RequestDuration { get; set; }
    public List<Product> ProductsList { get; set; }
}