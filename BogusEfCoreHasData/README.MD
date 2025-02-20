# Demo content
1. Run database in docker (`docker compose up -d`)
2. Run `dotnet ef database update` if it doesn't exist yet
3. Talk about this being a console application for a product database. 
   We are going to showcase the products in the database and seed it in different ways.
4. Run application and show 0 items in database
5. Showcase entering items into database by hand (Slow, not repeatable)
6. **Ensure that data is deleted before continuing**
7. Showcase using HasData manually by uncommenting that method in `ProductContext`.
8. Run `dotnet ef migrations add AddedManualSeedData` and show off the migration
9. Run `dotnet ef database update`
10. Run application and show items in database
11. Now run `dotnet ef migrations remove --force` to remove the migration
12. Now showcase Bogus by doing the following:
	3. Comment `SetupManualSeedData` in `ProductContext`
	4. Uncomment  `SetupSeedDataWithBogus` in `ProductContext`
	5. Run `dotnet ef migrations add AddedBogusSeedData`
	6. Run `dotnet ef database update`
	7. Run the application and showcase it.
13. Now showcase changing the model and things still working:
	1. Uncomment `Description` in `Product.cs`
	2. Uncomment `Description` in `GetRowsToRender` and `DrawTable` in `Worker.cs`
	2. Uncomment line 30 in `DatabaseSeeder.cs`
	3. Run `dotnet ef migrations add AddedDescriptionToProductWithSeedData`
    4. Run `dotnet ef database update`
	5. Run the application and showcase it.
14. Done?