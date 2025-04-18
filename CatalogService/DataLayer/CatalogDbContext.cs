using BRD.WebStore.Catalog.Infrastructure.Entities;
using BRD.WebStore.Catalog.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace BRD.WebStore.Catalog.DataLayer;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options, IUserContext userContext) : DbContext(options), ICatalogDbContext
{
    private IUserContext _userContext = userContext;

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductsCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Category>(builder =>
        {
            builder.ConfigureAuditableEntity();
            builder.HasOne(e => e.ParentCategory)
                   .WithMany(e => e.SubCategories)
                   .HasForeignKey(e => e.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.ConfigureAuditableEntity();
        });

        modelBuilder.Entity<ProductCategory>(builder =>
        {
            builder.HasKey(pc => new { pc.ProductId, pc.CategoryId });
            builder.HasOne(e => e.Category)
                   .WithMany(e => e.ProductCategories)
                   .HasForeignKey(e => e.CategoryId);
            builder.HasOne(e => e.Product)
                   .WithMany(e => e.ProductCategories)
                   .HasForeignKey(e => e.ProductId);
        });

    }

    public override int SaveChanges()
    {
        HandleAuditableEntity();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleAuditableEntity();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleAuditableEntity()
    {
        var utcNow = DateTime.UtcNow;

        var entities = ChangeTracker.Entries<AuditableEntity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entities)
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedByUserId = _userContext.CurrentUserId;
                entry.Entity.CreatedAt = utcNow;
            }

            entry.Entity.UpdatedByUserId = _userContext.CurrentUserId;
            entry.Entity.UpdatedAt = utcNow;
        }
    }
}

internal static class EntityTypeBuilderExtensions
{
    internal static void ConfigureAuditableEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : AuditableEntity
    {
        builder.Property(p => p.CreatedAt)
            .HasColumnType("TIMESTAMPTZ")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("TIMESTAMPTZ")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
