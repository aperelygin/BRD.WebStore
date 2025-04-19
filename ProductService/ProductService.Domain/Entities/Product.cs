using ProductService.Domain.Enums;

namespace ProductService.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    public ProductStatus Status { get; private set; }

    // EF ctor
    private Product() { }

    public Product(Guid id, string name, string description, decimal price, int stock)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stock;
        Status = ProductStatus.Active;
    }

    public void UpdateStock(int quantity)
    {
        StockQuantity = quantity;
        // можно поднять доменное событие — ProductStockChangedEvent
    }
}