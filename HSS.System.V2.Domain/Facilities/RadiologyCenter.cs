using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.People;
using HSS.System.V2.Domain.Medical;
using HSS.System.V2.Domain.Queues;

namespace HSS.System.V2.Domain.Facilities;

public class RadiologyCenter : BaseClass, IHospitalDepartmentItem
{
    public string Name { get; set; }
    public int NumberOfShifts { get; set; }
    public TimeSpan StartAt { get; set; }
    public TimeSpan EndAt { get; set; }
    public string QueueId { get; set; }
    [ForeignKey(nameof(QueueId))]
    public RadiologyCenterQueue Queue { get; set; }
    public TimeSpan PeriodPerAppointment { get; set; }

    public string? CurrentWorkingTesterId { get; set; }
    [ForeignKey(nameof(CurrentWorkingTesterId))]
    public virtual RadiologyTester CurrentWorkingTester { get; set; }

    public string HospitalId { get; set; }
    [ForeignKey(nameof(HospitalId))]
    public virtual Hospital Hospital { get; set; }

    public virtual ICollection<RadiologyTest> Tests { get; set; }

    [InverseProperty(nameof(RadiologyTester.RadiologyCenter))]
    public virtual ICollection<RadiologyTester> RadiologyTesters { get; set; }
} 