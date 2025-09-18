using Care.Models;

namespace Care.Data;

public static class Seed
{
    public static void Initialize(AppDbContext context)
    {
        // If user already exists, skip
        if (context.Users.Any()) return;

        // Add a test user
        var testUser = new User
        {
            Email = "test@example.com",
            Password = "1234" // In production: hash this!
        };

        context.Users.Add(testUser);
        context.SaveChanges();
    }
}