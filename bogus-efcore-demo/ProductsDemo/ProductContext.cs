using Microsoft.EntityFrameworkCore;

namespace ProductsDemo;

public class ProductContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductProductCategory> ProductProductCategories => Set<ProductProductCategory>();

    public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<ProductProductCategory>().HasKey(x => new { x.ProductId, x.CategoryId });

        // Old fashioned way: (TODO SET UP)
        //modelBuilder.Entity<Product>().HasData(new Product { Id = 1, CreationDate = DateTimeOffset.Now, Name = "Headphones", ProductProductCategories = new ProductCategory[] { new ProductCate () { } } })


        // STEP: Enable this to showcase Bogus
        //// Generate seed data with Bogus
        //var databaseSeeder = new DatabaseSeeder();

        //// Apply the seed data on the tables
        //modelBuilder.Entity<Product>().HasData(databaseSeeder.Products);
        //modelBuilder.Entity<ProductCategory>().HasData(databaseSeeder.ProductCategories);
        //modelBuilder.Entity<ProductProductCategory>().HasData(databaseSeeder.ProductProductCategories);


        base.OnModelCreating(modelBuilder);
    }
}