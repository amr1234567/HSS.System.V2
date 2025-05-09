using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Domain.Helpers.Models
{
    public class DateFilterationRequest
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateFilterationRequest(DateTime? dateFrom, DateTime? dateTo)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
        public DateFilterationRequest(DateTime? dateFrom)
        {
            DateFrom = dateFrom;
        }
        public DateFilterationRequest()
        {
            
        }
    }
}
