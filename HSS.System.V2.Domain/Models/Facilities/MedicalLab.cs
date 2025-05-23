using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Domain.Models.Facilities;

public class MedicalLab : BaseClass, IHospitalDepartmentItem, ITestableDepartment<MedicalLab, MedicalLabTest>
{
    public string Name { get; set; }
    public int NumberOfShifts { get; set; }
    public TimeSpan StartAt { get; set; }
    public TimeSpan EndAt { get; set; }
    public string? QueueId { get; set; }
    [ForeignKey(nameof(QueueId))]
    public virtual MedicalLabQueue Queue { get; set; }
    public TimeSpan PeriodPerAppointment { get; set; }
    public string CurrentWorkingTesterId { get; set; }
    [ForeignKey(nameof(CurrentWorkingTesterId))]
    public virtual MedicalLabTester CurrentWorkingTester { get; set; }
    public string HospitalId { get; set; }
    [ForeignKey(nameof(HospitalId))]
    public virtual Hospital Hospital { get; set; }

    public virtual ICollection<MedicalLabTest> Tests { set; get; }

    [InverseProperty(nameof(MedicalLabTester.MedicalLab))]
    public virtual ICollection<MedicalLabTester> MedicalLabTesters { get; set; }
    public virtual ICollection<MedicalLabAppointment> MedicalLabAppointments { get; set; }

    public Expression<Func<MedicalLab, IEnumerable<MedicalLabTest>>> GetInclude()
    {
        return x => x.Tests;
    }
    [NotMapped]
    public IEnumerable<MedicalLabTest> DepartmentTests => Tests;
} 