using Bogus;

namespace BogusUnitTests;

// Convert to class
public class User
{
    public required int Id { get; set; }
    public required bool IsActive { get; set; }
    public required DateTime DateCreated { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }

    // generate a trostring method
    public override string ToString()
    {
        return $"Id: {Id}, IsActive: {IsActive}, DateCreated: {DateCreated}, FirstName: {FirstName}, LastName: {LastName}, Email: {Email}";
    }
}

public class UserService
{
    public bool SaveUser(User user)
    {
        // Implementation doesn't matter..
        Console.WriteLine($"Saving user: {user}");

        return true;
    }
}

public class UserServiceTests
{
    private static int UserId = 1;

    [Test]
    public async Task SaveUser_Works_Successfully()
    {
        var service = new UserService();
        var faker = GetFaker();

        var user = faker.Generate();

        var result = service.SaveUser(user);
        await Assert.That(result).IsTrue();
    }

    private static Faker<User> GetFaker()
    {
        var userFaker = new Faker<User>()
            //.UseSeed(123) // You could use a seed to ensure deterministic behavior
            .UseDateTimeReference(DateTime.Parse("1/1/1982"))
            .StrictMode(true) // Ensure all known properties have rules
            .RuleFor(u => u.Id, f => UserId++)
            .RuleFor(u => u.IsActive, f => f.Random.Bool())
            .RuleFor(u => u.DateCreated, f => f.Date.Past())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, (f, usr) => f.Internet.Email(usr.FirstName, usr.LastName));

        return userFaker;
    }

}
