using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.DTOs.MedicalLabDTOs
{
    public class MedicaLabTestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double EstimatedDurationInMinutes { get; set; }
        public string SampleType { get; set; }
    }
}
