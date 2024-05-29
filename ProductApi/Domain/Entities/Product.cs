using ProductApi.Domain.ValueObjects;

namespace ProductApi.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public Money Price { get; set; } = null!;
}
