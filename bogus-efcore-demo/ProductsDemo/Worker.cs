using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace ProductsDemo;

public class Worker(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductContext>();

        var products = await dbContext.Products
            .Include(x => x.ProductProductCategories)
            .ThenInclude(x => x.Category)
            .ToListAsync();

        DrawTable(products);
    }

    private static void DrawTable(List<Product> products)
    {
        DrawLine();

        DrawBigText("Demo");

        // Creating the table
        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        // DEMO: Uncomment this line
        //table.AddColumn("Description");
        table.AddColumn("Categories");
        table.AddColumn("CreationDate");

        // Populating the table with product entries
        foreach (var product in products.Take(20))
        {
            var getRowsToRender = GetRowsToRender(product);
            table.AddRow(getRowsToRender);
        }

        table.Centered();

        // Rendering the table
        AnsiConsole.Write(table);

        DrawBigText($"Total: {products.Count} products");

        static void DrawLine()
        {
            var rule = new Rule("[green]EF <3 Bogus[/]");
            AnsiConsole.Write(rule);
        }

        static void DrawBigText(string text)
        {
            AnsiConsole.Write(new FigletText(text).Centered());
        }
    }

    private static string[] GetRowsToRender(Product product) => [
        product.Id.ToString(),
        product.Name,
        // DEMO: Uncomment this line
        //product.Description,
        string.Join(",", product.ProductProductCategories.Select(x => x.Category.Name)),
        product.CreationDate.ToString()
    ];
}
