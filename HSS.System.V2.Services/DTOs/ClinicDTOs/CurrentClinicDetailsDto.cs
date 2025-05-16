using HSS.System.V2.Services.DTOs.ReceptionDTOs;

namespace HSS.System.V2.Application.DTOs.Clinic
{
    public class CurrentClinicDetailsDto
    {
        public ClinicDetailsDto ClinicDetails { get; set; }
        public IEnumerable<AppointmentDto> TodayAppointments { get; set; }
        public IEnumerable<RadiologyCenterDto> TodayRadiology { get; set; }
        public int ActivePatientsCount { get; set; }
        public int AvailableDoctorsCount { get; set; }
        public object RadiologyOrders { get; internal set; }
        public object Clinic { get; internal set; }
    }
}