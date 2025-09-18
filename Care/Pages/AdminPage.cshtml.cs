using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Care.Data;
using Care.Models;

namespace Care.Pages;

public class AdminPageModel : PageModel
{
    private readonly AppDbContext _context;

    public AdminPageModel(AppDbContext context)
    {
        _context = context;
    }

    public List<User> Users { get; set; } = new();

    public IActionResult OnGet()
    {
        // Check if the current user is an admin
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToPage("/SignIn");

        var currentUser = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (currentUser == null || !currentUser.IsAdmin)
            return RedirectToPage("/AccessDenied");

        // Load all users
        Users = _context.Users.ToList();
        return Page();
    }

    public IActionResult OnPostDelete(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToPage("/SignIn");

        var currentUser = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (currentUser == null || !currentUser.IsAdmin)
            return RedirectToPage("/AccessDenied");

        var userToDelete = _context.Users.FirstOrDefault(u => u.Id == id);
        if (userToDelete != null)
        {
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
        }

        return RedirectToPage();
    }
}