using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Enums;

namespace HSS.System.V2.Domain.People;

public class Person : BaseClass
{
    public string Name { get; set; }
    public DateTime BirthOfDate { get; set; }
    public string? Email { get; set; }
    public string NationalId { get; set; }
    public string Address { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string Salt { get; set; }
    public string HashPassword { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpirationDate { get; set; }
    public UserRole Role { get; set; }
} 