using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Care.Data;
using Microsoft.EntityFrameworkCore;

namespace Care;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.AddRazorPages();
        builder.Services.AddAuthorization();
        builder.Services.AddSession();
        builder.Services.AddHttpContextAccessor();

        // Register SQLite DB context
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Seed DB
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            Seed.Initialize(db);
        }

        // Middleware
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthorization();
        app.UseSession(); // Must be after UseRouting and before MapRazorPages

        app.MapRazorPages();

        // Listen on Render's dynamic port
        var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
        app.Urls.Add($"http://*:{port}");

        app.Run();
    }
}