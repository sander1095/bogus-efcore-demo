using Bogus;

namespace BogusEfCoreUseSeeding;

public class DatabaseSeeder
{
    public List<Product> Products { get; } = [];
    public List<ProductCategory> ProductCategories { get; } = [];
    public List<ProductProductCategory> ProductProductCategories { get; } = [];

    public DatabaseSeeder()
    {
        // This project performs data existance checks with the name, so this has a unique index.
        // Bogus doesn't have a big enough dataset to guarantee unique names for a GIANT data set, so I just limited the numbers here for ease of use.
        Products = GenerateProducts(amount: 100);
        ProductCategories = GenerateProductCategories(amount: 22);
        ProductProductCategories = GenerateProductProductCategories(amount: 200, Products, ProductCategories);
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
        var categoryFaker = new Faker<ProductCategory>()
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
            .RuleFor(x => x.Product, f => f.PickRandom(products))
            .RuleFor(x => x.Category, f => f.PickRandom(productCategories));

        var productProductCategories = Enumerable.Range(1, amount)
            .Select(i => SeedRow(productProductCategoryFaker, i))
            // We do this GroupBy() + Select() to remove the duplicates from the generated join table entities
            .GroupBy(x => new { ProductName = x.Product.Name, CategoryName = x.Category.Name })
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
