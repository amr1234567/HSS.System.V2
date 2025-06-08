using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.Common;

namespace HSS.System.V2.Domain.Models.People;

public class Person : BaseClass
{
    public string Name { get; set; }
    public DateTime BirthOfDate { get; set; }
    public string? UrlOfProfilePicutre { get; set; }
    public string? Email { get; set; }
    public string NationalId { get; set; }
    public string? Address { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; }

    public int GetAge()
    {
        // Check if NationalId is null, empty, or not 14 digits
        if (string.IsNullOrEmpty(NationalId) || NationalId.Length != 14)
        {
            throw new InvalidOperationException("Invalid national ID");
        }

        // Extract components from the NationalId string
        char centuryDigit = NationalId[0];
        string yearDigits = NationalId.Substring(1, 2); // Digits 2-3
        string monthDigits = NationalId.Substring(3, 2); // Digits 4-5
        string dayDigits = NationalId.Substring(5, 2);   // Digits 6-7

        // Determine the full year based on the century digit
        int year;
        if (centuryDigit == '2')
        {
            year = 1900 + int.Parse(yearDigits);
        }
        else if (centuryDigit == '3')
        {
            year = 2000 + int.Parse(yearDigits);
        }
        else
        {
            throw new InvalidOperationException("Invalid century digit in national ID");
        }

        // Parse month and day
        int month = int.Parse(monthDigits);
        int day = int.Parse(dayDigits);

        // Create and return the DateTime object, handling invalid dates
        try
        {
            //return new DateTime(year, month, day);
            return DateTime.UtcNow.Year - year;
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new InvalidOperationException("Invalid date in national ID");
        }
    }

}