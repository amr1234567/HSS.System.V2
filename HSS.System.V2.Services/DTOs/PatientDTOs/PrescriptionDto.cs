using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public class PrescriptionDto
    {
        public string PrescriptionId { get; set; }
        public string ClinicName  { get; set; }
        public string? DoctorName  { get; set; }
        public string HospitalName  { get; set; }
        public int MedicineCount { get; set; }
        public DateTime? Date { get; set; }
    }
}
