using Microsoft.EntityFrameworkCore;

namespace BogusEfCoreUseSeeding;

public class ProductContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductProductCategory> ProductProductCategories => Set<ProductProductCategory>();

    public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<ProductCategory>().HasIndex(x => x.Name).IsUnique();

        // Necessary to use a composite key
        modelBuilder.Entity<ProductProductCategory>().HasKey(x => new { x.ProductId, x.CategoryId });

        base.OnModelCreating(modelBuilder);
    }
}