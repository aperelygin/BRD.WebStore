namespace BRD.WebStore.Catalog.Infrastructure.Entities;

public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
}
