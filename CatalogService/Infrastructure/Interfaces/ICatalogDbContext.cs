using BRD.WebStore.Catalog.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BRD.WebStore.Catalog.Infrastructure.Interfaces;

public interface ICatalogDbContext
{
    DbSet<Category> Categories { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<ProductCategory> ProductsCategories { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
