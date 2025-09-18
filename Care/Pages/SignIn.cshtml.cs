using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Care.Data;
using Care.Models;
using Microsoft.AspNetCore.Http;

namespace Care.Pages;

public class SignInModel : PageModel
{
    private readonly AppDbContext _context;

    public SignInModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string LoginIdentifier { get; set; } = ""; // Can be email or username

    [BindProperty]
    public string Password { get; set; } = "";

    public bool LoginFailed { get; set; } = false;

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        var user = _context.Users.FirstOrDefault(u =>
            (u.Email == LoginIdentifier || u.Username == LoginIdentifier) &&
            u.Password == Password);

        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("IsAdmin", user.IsAdmin ? 1 : 0); //Store IsAdmin in session
            return RedirectToPage("/MainPage");
        }

        LoginFailed = true;
        return Page();
    }
}