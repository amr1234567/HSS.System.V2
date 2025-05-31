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
        var today = DateTime.Today;
        var age = today.Year - BirthOfDate.Year;

        if (BirthOfDate.Date > today.AddYears(-age))
            age--;

        return age;
    }

}