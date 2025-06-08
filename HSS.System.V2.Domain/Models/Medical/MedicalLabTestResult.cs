using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Domain.Models.Medical
{
    public class MedicalLabTestResult : BaseClass
    {
        public string AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public virtual MedicalLabAppointment Appointment { get; set; }
        public string FieldId { get; set; }
        [ForeignKey(nameof(FieldId))]
        public virtual MedicalLabTestResultField MedicalLabTestResultField { get; set; }
        public string Value { get; set; }
    }
}
