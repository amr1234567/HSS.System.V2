using Domain.Models.Appointments;

using SharedServices.Helpers;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class RoomBookingResultDto : IOutputDto<RoomBookingResultDto, RoomBooking>
    {
        public string? PatientName { get; set; } = "غير معروف";
        public double PaidAmount { get; set; }
        public double Salary { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan RemainingPeriod { get; set; }
        public DateTime StartAt { get; set; }

        public RoomBookingResultDto MapFromModel(RoomBooking model)
        {
            PatientName = model.Patient?.FirstName + " " + model.Patient?.LastName;
            PaidAmount = model.PaidAmount;
            Salary = model.Salary;
            Duration = model.Duration;
            StartAt = model.StartAt;
            RemainingPeriod = CalculateRemainingPeriod(model.StartAt, model.Duration);
            return this;
        }

        private TimeSpan CalculateRemainingPeriod(DateTime startAt, TimeSpan duration)
        {
            var endTime = startAt.Add(duration);
            var remainingTime = endTime - DateTime.Now;
            return remainingTime.TotalMilliseconds > 0 ? remainingTime : TimeSpan.Zero;
        }
    }
}
