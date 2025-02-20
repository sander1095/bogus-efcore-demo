using Microsoft.EntityFrameworkCore;

namespace BogusEfCoreHasData;

public class ProductContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductProductCategory> ProductProductCategories => Set<ProductProductCategory>();

    public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Necessary to use a composite key
        modelBuilder.Entity<ProductProductCategory>().HasKey(x => new { x.ProductId, x.CategoryId });

        // DEMO: Uncomment this line
        //SetupManualSeedData(modelBuilder);

        // DEMO: Uncomment this line
        //SetupSeedDataWithBogus(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void SetupManualSeedData(ModelBuilder modelBuilder)
    {
        var product1 = new Product
        {
            Id = 1,
            Name = "Laptop",
            CreationDate = DateTimeOffset.Parse("2024-04-15T18:00:00+01:00")
        };

        var product2 = new Product
        {
            Id = 2,
            Name = "C# In Depth",
            CreationDate = DateTimeOffset.Parse("2024-04-15T19:00:00+01:00")
        };

        var category1 = new ProductCategory() { Id = 1, Name = "Technology" };
        var category2 = new ProductCategory() { Id = 2, Name = "Books" };


        var joinedCategory1 = new ProductProductCategory { ProductId = product1.Id, CategoryId = category1.Id };
        var joinedCategory2 = new ProductProductCategory { ProductId = product2.Id, CategoryId = category1.Id };
        var joinedCategory3 = new ProductProductCategory { ProductId = product2.Id, CategoryId = category2.Id };

        modelBuilder.Entity<Product>().HasData(product1, product2);
        modelBuilder.Entity<ProductCategory>().HasData(category1, category2);
        modelBuilder.Entity<ProductProductCategory>().HasData(joinedCategory1, joinedCategory2, joinedCategory3);
    }


    private static void SetupSeedDataWithBogus(ModelBuilder modelBuilder)
    {
        // Generate seed data with Bogus
        var databaseSeeder = new DatabaseSeeder();

        // Apply the seed data on the tables
        modelBuilder.Entity<Product>().HasData(databaseSeeder.Products);
        modelBuilder.Entity<ProductCategory>().HasData(databaseSeeder.ProductCategories);
        modelBuilder.Entity<ProductProductCategory>().HasData(databaseSeeder.ProductProductCategories);
    }
}