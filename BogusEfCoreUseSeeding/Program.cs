using BogusEfCoreUseSeeding;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Add Aspire service defaults for OpenTelemetry, health checks, and logging
builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ProductContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("Database")!)
    .UseSeeding((context, _) =>
    {
        var databaseSeeder = new DatabaseSeeder();
        var productSet = context.Set<Product>();
        var productProductCategorySet = context.Set<ProductProductCategory>();
        var productCategorySet = context.Set<ProductCategory>();

        foreach (var product in databaseSeeder.Products)
        {
            if (!productSet.Any(x => x.Name == product.Name))
            {
                productSet.Add(product);
            }
        }

        foreach (var productProductCategory in databaseSeeder.ProductProductCategories)
        {
            if (!productProductCategorySet.Any(x => x.ProductId == productProductCategory.ProductId && x.CategoryId == productProductCategory.CategoryId))
            {
                productProductCategorySet.Add(productProductCategory);
            }
        }

        foreach (var productCategory in databaseSeeder.ProductCategories)
        {
            if (!productCategorySet.Any(x => x.Name == productCategory.Name))
            {
                productCategorySet.Add(productCategory);
            }
        }

        context.SaveChanges();
    })
    .UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var databaseSeeder = new DatabaseSeeder();
        var productSet = context.Set<Product>();
        var productProductCategorySet = context.Set<ProductProductCategory>();
        var productCategorySet = context.Set<ProductCategory>();

        foreach (var product in databaseSeeder.Products)
        {
            if (!await productSet.AnyAsync(x => x.Name == product.Name, cancellationToken))
            {
                productSet.Add(product);
            }
        }

        foreach (var productProductCategory in databaseSeeder.ProductProductCategories)
        {
            if (!await productProductCategorySet.AnyAsync(x =>
                x.ProductId == productProductCategory.ProductId &&
                x.CategoryId == productProductCategory.CategoryId, cancellationToken)
            )
            {
                productProductCategorySet.Add(productProductCategory);
            }
        }

        foreach (var productCategory in databaseSeeder.ProductCategories)
        {
            if (!await productCategorySet.AnyAsync(x => x.Name == productCategory.Name, cancellationToken))
            {
                productCategorySet.Add(productCategory);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    });
});

var host = builder.Build();

// Uncomment to run the database seeding on startup
//await using var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
//await scope.ServiceProvider.GetRequiredService<ProductContext>().Database.EnsureCreatedAsync();

host.Run();
