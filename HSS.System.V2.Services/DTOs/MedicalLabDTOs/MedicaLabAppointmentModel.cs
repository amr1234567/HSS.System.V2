using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.DTOs.MedicalLabDTOs
{
    public class MedicaLabAppointmentModel
    {
        public string AppointmentId { get; set; }
        public string PatientNationalId { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string LastAppointmentDiagnosis { get; set; }
        public string TestNeededId { get; set; }
        public string TestNeededName { get; set; }
    }
}
