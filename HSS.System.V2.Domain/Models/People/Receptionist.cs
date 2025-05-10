using System.ComponentModel.DataAnnotations.Schema;

using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Domain.Models.People;

public class Receptionist : Employee
{
    public string ReceptionId { get; set; }
    [ForeignKey(nameof(ReceptionId))]
    public virtual Reception Reception { get; set; }
} 