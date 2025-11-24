using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add a SQL Server container for demo purposes
var sqlServer = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent);

// Create databases for each demo project
var hasDataDb = sqlServer.AddDatabase("hasdata-db");
var useSeedingDb = sqlServer.AddDatabase("useseeding-db");

// Add BogusEfCoreHasData project with custom commands for demo scenarios
var bogusHasData = builder.AddProject<BogusEfCoreHasData>("bogus-efcore-hasdata")
    .WithReference(hasDataDb)
    .WaitFor(hasDataDb)
    .WithCommand(
        name: "run-manual-seed",
        displayName: "Run Manual Seed Data Demo",
        executeCommand: async context =>
        {
            // This command serves as a reminder for how to use this demo scenario
            // To use: Uncomment 'SetupManualSeedData(modelBuilder)' in ProductContext.cs
            // Then run 'dotnet ef migrations add ManualSeedData' followed by 'dotnet ef database update'
            return CommandResults.Success();
        },
        commandOptions: new CommandOptions
        {
            IconName = "Database",
            IconVariant = IconVariant.Filled
        })
    .WithCommand(
        name: "run-bogus-seed",
        displayName: "Run Bogus Seed Data Demo",
        executeCommand: async context =>
        {
            // This command serves as a reminder for how to use this demo scenario
            // To use: Uncomment 'SetupSeedDataWithBogus(modelBuilder)' in ProductContext.cs
            // Then run 'dotnet ef migrations add BogusSeedData' followed by 'dotnet ef database update'
            return CommandResults.Success();
        },
        commandOptions: new CommandOptions
        {
            IconName = "DatabaseLightning",
            IconVariant = IconVariant.Filled
        });

// Add BogusEfCoreUseSeeding project with a "Seed Now" command
var bogusUseSeeding = builder.AddProject<BogusEfCoreUseSeeding>("bogus-efcore-useseeding")
    .WithReference(useSeedingDb)
    .WaitFor(useSeedingDb)
    .WithCommand(
        name: "seed-now",
        displayName: "Seed Database Now",
        executeCommand: async context =>
        {
            // The seeding is automatically handled by EF Core's UseSeeding/UseAsyncSeeding
            // when EnsureCreated or Migrate is called.
            // To seed: run 'dotnet ef database update' in the BogusEfCoreUseSeeding directory
            return CommandResults.Success();
        },
        commandOptions: new CommandOptions
        {
            IconName = "LeafTwo",
            IconVariant = IconVariant.Filled
        });

builder.Build().Run();
