using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Care.Data;
using Care.Models;
using System.Linq;

public class MainPageModel : PageModel
{
    private static readonly HttpClient _httpClient = new();
    private readonly AppDbContext _context;

    public MainPageModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string UserPrompt { get; set; }

    public string GeminiResponse { get; set; }

    public IActionResult OnGet()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToPage("/SignIn");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToPage("/SignIn");
        }

        if (string.IsNullOrWhiteSpace(UserPrompt))
        {
            GeminiResponse = "Please enter a prompt.";
            return Page();
        }

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            GeminiResponse = "User data not found.";
            return Page();
        }
//{user.Country}
        string fullPrompt = $"אתה כעת רופא מורשה בעל מומחיות ברפואה כללית. יוצגו בפניך שאלות של מטופלים בנוגע לסימפטומים, מחלות ותרופות. השב בהתבסס על הידע הרפואי הנוכחי שלך ושיקול הדעת הקליני הטוב ביותר. אם אינך בטוח לגבי אבחנה או אם הסימפטומים דורשים הערכה רפואית מקצועית, ציין בבירור שאינך יכול לאשר את המצב והמלץ בחום למטופל להתייעץ עם גורם רפואי מוסמך באופן אישי. במידה והמטופל שואל אותך שאלה או מדבר איתך על משהו שלא קשור לרפואה בכללי תסרב לענות לו ותכתוב לו לשאול אותך רק דברים שקשורים לרפואה, כי אתה רופא. בתשובתך, קודם התחל עם מה שלומך, ואז התחל בפסקה קצרה בת 2 משפטים המסבירה מה עשויים להעיד הסימפטומים של המטופל, בהתבסס על המידע שנמסר. השתמש בשפה רפואית נכונה תוך שמירה על בהירות ואמפתיה. לאחר מכן, ספק לפחות 3 נקודות תבליט ספציפיות וניתנות ליישום הכוללות אפשרויות טיפול פוטנציאליות, עצות לאורח חיים, תרופות ללא מרשם, או צעדים שיש לנקוט בהמשך. אם המצב נפוץ, המלץ על תרופה (לדוגמה, פאראצטמול לכאב ראש). בסיום תגובתך, כלול הצהרה ברורה כגון: \"אם הסימפטומים נמשכים יותר מ-X ימים, מחמירים, או מלווים בסימפטומים חדשים, אנא פנה לקבלת טיפול רפואי מיידי.\". הוראות עיצוב: רד שורה כל פעם שאתה מציג הצעה או כשאתה מציג שלב טיפול חדש. אל תכתוב כוכביות או סמלים מיוחדים למיניהם בשום אופן!, אל תכתוב נקודה בסוף משפט ואל תדגיש טקסט. מידע על המטופל (במידה וכתוב null - המידע אינו זמין): בעיות בריאותיות ידועות:  {user.KnownConditions} אלרגיות: {user.Allergies} גובה: {user.Height} משקל: {user.Weight} מין: {(user.Gender.HasValue ? (user.Gender.Value ? "זכר" : "נקבה") : "null")} מדינה: {user.Country} תאריך לידה: {(user.BirthDate?.ToString("yyyy-MM-dd") ?? "null")} שם פרטי: {user.FirstName} בעיית המטופל: {UserPrompt}\n";

        var apiKey = "AIzaSyBrdYPs3KkFpRPg-WnpFLziU33D1Ai4N5o";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = fullPrompt }
                    }
                }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        try
        {
            var response = await _httpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}",
                content);

            if (!response.IsSuccessStatusCode)
            {
                GeminiResponse = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                return Page();
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseBody);

            GeminiResponse = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();
        }
        catch (Exception ex)
        {
            GeminiResponse = $"Exception: {ex.Message}";
        }

        return Page();
    }
}