namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public class NotificationDto
    {
        public string Id { get; set; }
        public NotificationMessage? MessageData { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
