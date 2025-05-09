using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.People;

namespace HSS.System.V2.Domain.Facilities;

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
} 