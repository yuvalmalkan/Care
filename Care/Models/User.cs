using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Care.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = "";

    [Required]
    public string LastName { get; set; } = "";

    [Required]
    public string Username { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    public DateTime? BirthDate { get; set; } // optional

    [NotMapped]
    public int? Age => BirthDate == null ? null : 
        DateTime.Today.Year - BirthDate.Value.Year -
        (BirthDate.Value.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - BirthDate.Value.Year)) ? 1 : 0);

    public string? Country { get; set; }

    public bool? Gender { get; set; }

    public float? Weight { get; set; }

    public float? Height { get; set; }

    public string? Allergies { get; set; }

    public string? KnownConditions { get; set; }

    [Phone]
    public string? MobileNumber { get; set; }

    public bool IsAdmin { get; set; } = false;
}