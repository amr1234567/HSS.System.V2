using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.People;

namespace HSS.System.V2.Domain.Common;

public class LoginActivity : BaseClass
{
    public string EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public Employee Employee { get; set; }
    public ActivityType ActivityType { get; set; }
}
