using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Care.Pages;

public class Logout : PageModel
{
    public IActionResult OnGet()
    {
        HttpContext.Session.Clear();

        return RedirectToPage("/SignIn");
    }
}