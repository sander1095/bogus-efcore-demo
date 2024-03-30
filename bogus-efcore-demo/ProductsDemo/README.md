# Demo content
- Run database in docker (`docker compose up -d`)
- Run `dotnet ef database update` if it doesn't exist yet
- Showcase entering items into database by hand (Slow, not repeatable)
- Showcase using HasData manually by uncommenting that code in `ProductContext`
  - run `dotnet ef migrations add AddSeedDataByHand`
-
- Demo what you would normally do to set up testing data
- 