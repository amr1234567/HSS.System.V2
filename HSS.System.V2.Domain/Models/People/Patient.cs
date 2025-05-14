using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.Domain.Models.People;

public class Patient : Person
{
    public double Lat { get; set; }
    public double Lng { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; }
    public virtual ICollection<MedicalHistory> MedicalHistories { get; set; }
} 