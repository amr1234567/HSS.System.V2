using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.Domain.Models.Facilities;

public class Hospital : BaseClass
{
    public string Name { get; set; }
    public string Address { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public TimeSpan OpenAt { get; set; }
    public TimeSpan EndAt { get; set; }

    public virtual ICollection<Employee> Employees { get; set; }
    public virtual ICollection<Clinic> Clinics { get; set; }
    public virtual ICollection<RadiologyCenter> RadiologyCenters { get; set; }
    public virtual ICollection<MedicalLab> MedicalLabs { get; set; }
    public virtual ICollection<Reception> Receptions { get; set; }
    public virtual ICollection<Pharmacy> Pharmacies { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
} 