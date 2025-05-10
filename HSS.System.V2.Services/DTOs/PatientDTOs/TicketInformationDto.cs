
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public class TicketInformationDto
    {
        public string TicketId { get; set; }
        public DateTime CreateAt { get; set; }
        public string CurrentState { get; set; }
    }
}
