using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.People;

namespace HSS.System.V2.Domain.Models.Common;

public class LoginActivity : BaseClass
{
    public string EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public virtual Employee Employee { get; set; }
    public ActivityType ActivityType { get; set; }
}
