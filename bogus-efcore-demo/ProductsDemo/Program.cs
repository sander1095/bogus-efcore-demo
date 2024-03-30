using Microsoft.EntityFrameworkCore;
using ProductsDemo;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ProductContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("Database")!);
});

var host = builder.Build(); 

host.Run();
