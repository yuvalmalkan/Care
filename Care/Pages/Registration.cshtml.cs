using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Care.Models;
using Care.Data;
using Microsoft.AspNetCore.Http;

namespace Care.Pages;

public class RegistrationModel : PageModel
{
    private readonly AppDbContext _context;

    public RegistrationModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty] public string fName { get; set; } = "";
    [BindProperty] public string lName { get; set; } = "";
    [BindProperty] public string uEmail { get; set; } = "";
    [BindProperty] public string uPass { get; set; } = "";
    [BindProperty] public string uMobile { get; set; } = "";
    [BindProperty] public string uUser { get; set; } = "";
    [BindProperty] public string? uCountry { get; set; }
    [BindProperty] public string? uGender { get; set; }
    [BindProperty] public float? uWeight { get; set; }
    [BindProperty] public float? uHeight { get; set; }
    [BindProperty] public string? uAllergies { get; set; }
    [BindProperty] public string? uConditions { get; set; }
    [BindProperty] public int? bDay { get; set; }
    [BindProperty] public int? bMonth { get; set; }
    [BindProperty] public int? bYear { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        // Server-side validations
        if (string.IsNullOrWhiteSpace(fName) || fName.Length < 2)
            ModelState.AddModelError("fName", "First name must be at least 2 characters");

        if (string.IsNullOrWhiteSpace(lName) || lName.Length < 2)
            ModelState.AddModelError("lName", "Last name must be at least 2 characters");

        if (string.IsNullOrWhiteSpace(uUser) || uUser.Length < 4)
            ModelState.AddModelError("uUser", "Username must be at least 4 characters");

        if (string.IsNullOrWhiteSpace(uPass) || !HasValidPassword(uPass))
            ModelState.AddModelError("uPass", "Password must include uppercase, lowercase letters, and digits");

        if (string.IsNullOrWhiteSpace(uEmail) || !uEmail.Contains("@") || !uEmail.Contains("."))
            ModelState.AddModelError("uEmail", "Invalid email format");

        if (string.IsNullOrWhiteSpace(uMobile) || uMobile.Length != 10 || !uMobile.All(char.IsDigit))
            ModelState.AddModelError("uMobile", "Mobile number must be exactly 10 digits");

        if (string.IsNullOrWhiteSpace(uCountry))
            ModelState.AddModelError("uCountry", "Country is required");

        if (string.IsNullOrWhiteSpace(uGender))
            ModelState.AddModelError("uGender", "Gender is required");

        if (uWeight == null || uWeight <= 0)
            ModelState.AddModelError("uWeight", "Weight must be a positive number");

        if (uHeight == null || uHeight <= 0)
            ModelState.AddModelError("uHeight", "Height must be a positive number");

        if (bDay == null || bMonth == null || bYear == null)
            ModelState.AddModelError("bDay", "Complete birthdate is required");
        else
        {
            DateTime parsedBirthDate;
            try
            {
                parsedBirthDate = new DateTime(bYear.Value, bMonth.Value, bDay.Value);
            }
            catch
            {
                ModelState.AddModelError("", "Invalid birth date.");
                return Page();
            }
        }

        if (!ModelState.IsValid)
        {
            return Page(); // Return with field errors
        }

        // All validations passed — create user
        DateTime birthDate = new DateTime(bYear.Value, bMonth.Value, bDay.Value);
        bool genderBool = uGender == "true";

        var newUser = new User
        {
            FirstName = fName,
            LastName = lName,
            Email = uEmail,
            Password = uPass,
            MobileNumber = uMobile,
            Username = uUser,
            Country = uCountry,
            Gender = genderBool,
            Weight = uWeight.Value,
            Height = uHeight.Value,
            Allergies = string.IsNullOrWhiteSpace(uAllergies) ? null : uAllergies,
            KnownConditions = string.IsNullOrWhiteSpace(uConditions) ? null : uConditions,
            BirthDate = birthDate,
            IsAdmin = false
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("UserId", newUser.Id);
        HttpContext.Session.SetString("Username", newUser.Username);

        return RedirectToPage("/MainPage");
    }

    private bool HasValidPassword(string password)
    {
        return password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit);
    }
}