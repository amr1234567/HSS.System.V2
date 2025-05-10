using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.People;
using HSS.System.V2.Domain.Medical;
using HSS.System.V2.Domain.Prescriptions;
using HSS.System.V2.Domain.Queues;
using HSS.System.V2.Domain.Common;

namespace HSS.System.V2.Domain.Appointments;

public class ClinicAppointment : Appointment, IAppointmentModel<ClinicQueue>
{
    public string Result { get; set; }
    public string Diagnosis { get; set; }
    public bool ReExaminationNeeded { get; set; }
    public string? ReExamiationClinicAppointemntId { get; set; }
    public virtual ClinicAppointment ReExamiationClinicAppointemnt { get; set; }
    public string? PreExamiationClinicAppointemntId { get; set; }
    public virtual ClinicAppointment PreExamiationClinicAppointemnt { get; set; }
    public string? DiseaseId { get; set; }
    public virtual Disease Disease { get; set; }
    public string ClinicId { get; set; }
    public virtual Clinic Clinic { get; set; }
    public string DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; }
    public string? QueueId { get; set; }
    public ClinicQueue Queue { get; set; }
    public string PrescriptionId { get; set; }
    [ForeignKey(nameof(PrescriptionId))]
    public virtual Prescription Prescription { get; set; }

    public virtual ICollection<MedicalLabAppointment> MedicalLabAppointments { get; set; }
    public virtual ICollection<RadiologyCeneterAppointment> RadiologyCeneterAppointments { get; set; }
    public virtual ICollection<TestRequired> TestsRequired { get; set; }

    public void SetQueue(ClinicQueue? queue)
    {
        Queue = queue;
        QueueId = queue is null ? null : queue.Id;
    }

    public ClinicQueue? GetQueue()
    {
        return Queue;
    }
} 