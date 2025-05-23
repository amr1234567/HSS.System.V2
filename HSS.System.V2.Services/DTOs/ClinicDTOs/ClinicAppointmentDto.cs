namespace HSS.System.V2.Services.DTOs.ClinicDTOs;

public record ClinicAppointmentDto
{
    public string PatientId { get; set; }
    public string PatientName { get; set; }
    public string PatientNationalId { get; set; }
    public int PatientAge { get; set; }
    public string AppointmentId { get; set; }
    public DateTime ExpectedTimeToGetIn { get; set; }
    public string TicketId { get; set; }
}

