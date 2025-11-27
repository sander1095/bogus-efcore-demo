using Bogus;

namespace BogusEfCoreHasData;

public class DatabaseSeeder
{
    public List<Product> Products { get; } = [];
    public List<ProductCategory> ProductCategories { get; } = [];
    public List<ProductProductCategory> ProductProductCategories { get; } = [];

    public DatabaseSeeder()
    {
        Products = GenerateProducts(amount: 1000);
        ProductCategories = GenerateProductCategories(amount: 50);
        ProductProductCategories = GenerateProductProductCategories(amount: 1000, Products, ProductCategories);
    }

    private static List<Product> GenerateProducts(int amount)
    {
        var productId = 1;
        var productFaker = new Faker<Product>()
            .RuleFor(x => x.Id, f => productId++) // Each product will have an incrementing id.
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.CreationDate, f => (new DateTimeOffset(2024, 4, 15, 18, 0, 0, TimeSpan.FromHours(1))).Add(TimeSpan.FromMinutes(productId)));

        // DEMO: Uncomment this line
        //.RuleFor(x => x.Description, f => f.Commerce.ProductDescription());

        var products = Enumerable.Range(1, amount)
            .Select(i => SeedRow(productFaker, i))
            .ToList();

        return products;
    }

    private static List<ProductCategory> GenerateProductCategories(int amount)
    {
        var categoryId = 1;
        var categoryFaker = new Faker<ProductCategory>()
            .RuleFor(x => x.Id, f => categoryId++) // Each category will have an incrementing id.
            .RuleFor(x => x.Name, f => f.Commerce.Categories(1).First());

        var categories = Enumerable.Range(1, amount)
            .Select(i => SeedRow(categoryFaker, i))
            .ToList();

        return categories;
    }

    private static List<ProductProductCategory> GenerateProductProductCategories(
        int amount,
        IEnumerable<Product> products,
        IEnumerable<ProductCategory> productCategories)
    {
        // Now we set up the faker for our join table.
        // We do this by grabbing a random product and category that were generated.
        var productProductCategoryFaker = new Faker<ProductProductCategory>()
            .RuleFor(x => x.ProductId, f => f.PickRandom(products).Id)
            .RuleFor(x => x.CategoryId, f => f.PickRandom(productCategories).Id);

        var productProductCategories = Enumerable.Range(1, amount)
            .Select(i => SeedRow(productProductCategoryFaker, i))
            // We do this GroupBy() + Select() to remove the duplicates from the generated join table entities
            .GroupBy(x => new { x.ProductId, x.CategoryId })
            .Select(x => x.First())
            .ToList();

        return productProductCategories;
    }

    private static T SeedRow<T>(Faker<T> faker, int rowId) where T : class
    {
        var recordRow = faker.UseSeed(rowId).Generate();
        return recordRow;
    }
}
