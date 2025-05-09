using HSS.System.V2.Domain.Prescriptions;

namespace HSS.System.V2.Domain.People;

public class Patient : Person
{
    public double Lat { get; set; }
    public double Lng { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; }
} 