namespace BRD.WebStore.Catalog.Infrastructure.Entities;

public class ProductCategory
{
    public int ProductId { get; set; }
    public required Product Product { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}