using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public class NotificationMessage
    {
        public string? Title { get; set; }
        public string? Clinic { get; set; }
        public string? Doctor { get; set; }
    }

    public class NotificationData
    {
        public string? TestName { get; set; }
        public string? Hospital { get; set; }
        public DateTime? Date { get; set; }
    }
}
