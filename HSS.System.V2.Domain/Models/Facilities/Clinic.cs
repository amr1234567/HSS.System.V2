using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Domain.Models.Facilities;

public class Clinic : BaseClass, IHospitalDepartmentItem
{
    public string Name { get; set; }
    public int NumberOfShifts { get; set; }
    public TimeSpan StartAt { get; set; }
    public TimeSpan EndAt { get; set; }
    public string QueueId { get; set; }
    [ForeignKey(nameof(QueueId))]
    public ClinicQueue Queue { get; set; }
    public TimeSpan PeriodPerAppointment { get; set; }

    public string SpecializationId { get; set; }
    [ForeignKey(nameof(SpecializationId))]
    public virtual Specialization Specialization { get; set; }

    public string? CurrentWorkingDoctorId { get; set; }
    [ForeignKey(nameof(CurrentWorkingDoctorId))]
    public virtual Doctor CurrentWorkingDoctor { get; set; }

    public string HospitalId { get; set; }
    [ForeignKey(nameof(HospitalId))]
    public virtual Hospital Hospital { get; set; }

    [InverseProperty(nameof(Doctor.Clinic))]
    public virtual ICollection<Doctor> Doctors { get; set; }
    public virtual ICollection<ClinicAppointment> ClinicAppointments { get; set; }
} 