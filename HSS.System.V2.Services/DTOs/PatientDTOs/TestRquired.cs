using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public class TestRquired
    {
        public string? TestName  { get; set; }
        public string? ClinicName { get; set; }
        public string? DoctorName  { get; set; }
        public string? HospitalName { get; set; }
        public DateTime? Date { get; set; }
    } 
}
