using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Domain.Models.Requests
{
    public class DateFilteration
    {
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public bool ValidateDateRange()
        {
            if (DateFrom.HasValue && DateTo.HasValue)
            {
                return DateFrom.Value <= DateTo.Value;
            }
            return true;
        }
    }
} 