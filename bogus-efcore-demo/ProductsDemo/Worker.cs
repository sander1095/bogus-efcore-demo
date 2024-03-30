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

        var products = await dbContext.Products.Include(x => x.ProductProductCategories).ThenInclude(x => x.Category).ToListAsync();

        DrawTable(products);
    }

    private static void DrawTable(List<Product> products)
    {
        DrawLine();

        // Creating the table
        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Categories");

        // Populating the table with product entries
        foreach (var product in products)
        {
            table.AddRow(product.Id.ToString(), product.Name, string.Join(",", product.ProductProductCategories.Select(x => x.Category.Name)));
        }

        table.Centered();

        // Rendering the table
        AnsiConsole.Write(table);

        static void DrawLine()
        {
            var rule = new Rule("[green]EF <3 Bogus[/]");
            AnsiConsole.Write(rule);
            AnsiConsole.Write(new FigletText("Demo").Centered());
        }
    }

}
