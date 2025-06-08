using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.System.V2.Domain.Models.Medical;

public class MedicalLabTestResultFieldValue : BaseClass
{
    public string AppointmentId { get; set; }
    [ForeignKey(nameof(AppointmentId))]
    public virtual MedicalLabAppointment Appointment { get; set; }

    public string FieldId { get; set; }
    [ForeignKey(nameof(FieldId))]
    public virtual MedicalLabTestResultField MedicalLabTestResultField { get; set; }
    public ResultFieldType ResultFieldType { get; set; }

    public string Value { get; set; }
}
