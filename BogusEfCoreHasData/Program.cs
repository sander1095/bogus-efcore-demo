using BogusEfCoreHasData;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Add Aspire service defaults for OpenTelemetry, health checks, and logging
builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ProductContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("Database")!);
});

var host = builder.Build(); 

host.Run();
