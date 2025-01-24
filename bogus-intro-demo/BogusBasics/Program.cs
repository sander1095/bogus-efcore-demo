using System.Text;
using Bogus;
using BogusBasics;
using static System.Console;

public class User
{
   public int Id { get; set; }
   public bool IsActive { get; set; }
   public DateTime DateCreated { get; set; }
   public string FirstName { get; set; }
   public string LastName { get; set; }
   public string Email { get; set; }
}

public static class Program
{
   public static void Main()
   {
      int userId = 0;
      var userFaker = new Faker<User>()
         .UseSeed(777)
         .UseDateTimeReference(DateTime.Parse("1/1/1982"))
         .StrictMode(true) // Ensure all known properties have rules
         .RuleFor(u => u.Id,           f => userId++)
         .RuleFor(u => u.IsActive,     f => f.Random.Bool())
         .RuleFor(u => u.DateCreated,  f => f.Date.Past())
         .RuleFor(u => u.FirstName,    f => f.Name.FirstName())
         .RuleFor(u => u.LastName,     f => f.Name.LastName())
         .RuleFor(u => u.Email, (f, usr) => f.Internet.Email(usr.FirstName, usr.LastName));

      var user1 = userFaker.Generate();

      WriteLine("Using Faker<T>:");
      user1.Dump();


      WriteLine("\n-------------\n");

      var faker = new Faker()
         {
            DateTimeReference = DateTime.Parse("1/1/1982"),
            Random = new Randomizer(777)
         };
      
      userId = 0;
      var user2 = new User
         {
            Id = userId++,
            IsActive = faker.Random.Bool(),
            DateCreated = faker.Date.Past(),
            FirstName = faker.Name.FirstName(),
            LastName = faker.Name.LastName()
         };
      user2.Email = faker.Internet.Email(user2.FirstName, user2.LastName);

      WriteLine("Using Faker facade:");
      user2.Dump();
      
      ReadLine();
   }
}
