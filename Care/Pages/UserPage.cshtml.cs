using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Care.Data;
using Care.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Care.Pages;

public class UserPageModel : PageModel
{
    private readonly AppDbContext _context;

    public UserPageModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public UserInputModel Input { get; set; }

    public string FeedbackMessage { get; set; }
    public string FeedbackCssClass { get; set; }

    public class UserInputModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string MobileNumber { get; set; }

        public string? Country { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? Gender { get; set; }

        public float? Weight { get; set; }

        public float? Height { get; set; }

        public string? Allergies { get; set; }

        public string? KnownConditions { get; set; }
    }

    public IActionResult OnGet(int? id)
    {
        var sessionUserId = HttpContext.Session.GetInt32("UserId");
        if (sessionUserId == null)
            return RedirectToPage("/SignIn");

        var sessionUser = _context.Users.Find(sessionUserId);
        if (sessionUser == null)
            return RedirectToPage("/SignIn");

        int targetUserId = sessionUser.IsAdmin && id != null ? id.Value : sessionUser.Id;

        var user = _context.Users.Find(targetUserId);
        if (user == null)
            return RedirectToPage("/SignIn");

        Input = new UserInputModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            MobileNumber = user.MobileNumber,
            Country = user.Country,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            Weight = user.Weight,
            Height = user.Height,
            Allergies = user.Allergies,
            KnownConditions = user.KnownConditions
        };

        return Page();
    }

    public IActionResult OnPost()
    {
        var sessionUserId = HttpContext.Session.GetInt32("UserId");
        if (sessionUserId == null)
            return RedirectToPage("/SignIn");

        var sessionUser = _context.Users.Find(sessionUserId);
        if (sessionUser == null)
            return RedirectToPage("/SignIn");

        int targetUserId = sessionUser.IsAdmin ? Input.Id : sessionUser.Id;

        if (!ModelState.IsValid)
        {
            FeedbackMessage = "Please correct the highlighted errors.";
            FeedbackCssClass = "alert-danger";
            return Page();
        }

        var user = _context.Users.FirstOrDefault(u => u.Id == targetUserId);
        if (user == null)
            return RedirectToPage("/SignIn");

        // Check uniqueness for email and username
        bool usernameExists = _context.Users.Any(u => u.Username == Input.Username && u.Id != user.Id);
        bool emailExists = _context.Users.Any(u => u.Email == Input.Email && u.Id != user.Id);

        if (usernameExists)
        {
            ModelState.AddModelError("Input.Username", "Username already taken.");
            return Page();
        }

        if (emailExists)
        {
            ModelState.AddModelError("Input.Email", "Email already in use.");
            return Page();
        }

        user.FirstName = Input.FirstName;
        user.LastName = Input.LastName;
        user.Username = Input.Username;
        user.Email = Input.Email;
        user.MobileNumber = Input.MobileNumber;
        user.Country = Input.Country;
        user.BirthDate = Input.BirthDate;
        user.Gender = Input.Gender;
        user.Weight = Input.Weight;
        user.Height = Input.Height;
        user.Allergies = Input.Allergies;
        user.KnownConditions = Input.KnownConditions;

        _context.SaveChanges();

        if (!sessionUser.IsAdmin)
            HttpContext.Session.SetString("Username", user.Username);

        FeedbackMessage = "Details updated successfully!";
        FeedbackCssClass = "alert-success";

        return Page();
    }
}