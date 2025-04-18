namespace BRD.WebStore.Catalog.Infrastructure.Entities;

public class Category : AuditableEntity
{
    // Идентификатор категории.
    public int Id { get; set; }

    // Название категории.
    public string Name { get; set; } = string.Empty;

    // Описание категории.
    public string Description { get; set; } = string.Empty;

    // Идентификатор родительской категории, используемый для построения иерархии.
    public int? ParentCategoryId { get; set; }

    // Уровень категории в иерархии.
    public int DepthLevel { get; set; }

    // Порядок сортировки категории.
    public int SortOrder { get; set; }


    // Родительская категория.
    public Category? ParentCategory { get; set; }

    // Коллекция подкатегорий.
    public ICollection<Category>? SubCategories { get; set; }

    // Коллекция связей между категориями и продуктами.
    public ICollection<ProductCategory>? ProductCategories { get; set; }
}