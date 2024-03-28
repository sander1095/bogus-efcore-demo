using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var services = new ServiceCollection();
services.AddDbContext<BlogContext>(x => x.UseSqlite("Data Source=blog.db"));

var serviceProvider = services.BuildServiceProvider();

var dbContext = serviceProvider.GetRequiredService<BlogContext>();
dbContext.Database.Migrate();

// var blog = new Blog { Name = "Sander's .NET Blog" };
// var post = new Post
// {
//     Name = "EF Core ❤️ Bogus",
//     BlogId = blog.Id,
//     Blog = blog
// };

// blog.Posts.Add(post);

// dbContext.Blogs.Add(blog);
// dbContext.SaveChanges();

// List<Blog> allBlogs = dbContext.Blogs.Include(x => x.Posts).ToList();

public class BlogContext(DbContextOptions<BlogContext> options) : DbContext(options)
{
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>().HasData(
            new Blog { Id = 1, Name = "Default blog", Description = "hey" },
            new Blog { Id = 2, Name = "Default blog 2", Description = "hey" }
        );


        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    }
}

public class BlogContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<BlogContext>
{
    public BlogContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();
        optionsBuilder.UseSqlite("Data Source=bin/Debug/net8.0/blog.db");

        return new BlogContext(optionsBuilder.Options);
    }
}

public class Blog
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }

    public List<Post> Posts { get; set; } = [];
}

public class Post
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int BlogId { get; set; }
    public required Blog Blog { get; set; }
}