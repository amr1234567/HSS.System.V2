using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public class TicketViewDto
    {
        public string Id { get; set; }
        public string TicketState { get; set; }
        public int AppointmentCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
