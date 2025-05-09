using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Facilities;

namespace HSS.System.V2.Domain.People;

public class Employee : Person
{
    public TimeSpan StartAt { get; set; }
    public TimeSpan EndAt { get; set; }
    public string PositionName { get; set; }
    public double Salary { get; set; }
    public UserRole Role { get; set; }
    public virtual Hospital Hospital { get; set; }
    public string HospitalId { get; set; }
} 