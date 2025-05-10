using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.People;

namespace HSS.System.V2.Domain.Models.Notifications
{
    public class AppNotification : BaseClass
    {
        public string PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public string? MessageData { get; set; }
        public bool Seen { get; set; } = false;
        public string? Data { get; set; }
    }
}
