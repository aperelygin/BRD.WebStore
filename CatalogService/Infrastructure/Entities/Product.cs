namespace BRD.WebStore.Catalog.Infrastructure.Entities;

public class Product : AuditableEntity
{
    // Идентификатор продукта
    public int Id { get; set; } 
    // Название продукта
    public string Name { get; set; } = string.Empty;
    // Описание продукта
    public string Description { get; set; } = string.Empty;
    // Цена продукта
    public decimal Price { get; set; } 


    // Связи с категориями
    public ICollection<ProductCategory>? ProductCategories { get; set; }
    // Изображения продукта
    //public ICollection<ProductImage> Images { get; set; } 
    //// Характеристики продукта
    //public ICollection<ProductFeature> Features { get; set; } 
    //// Теги продукта
    //public ICollection<ProductTag> Tags { get; set; } 
    //// Запасы на разных складах
    //public ICollection<Inventory> Inventories { get; set; } 
}
