using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class MedicalLabDto : IOutputDto<MedicalLabDto, MedicalLab>
    {
        public TimeSpan StartAt { get; set; }
        public TimeSpan? EndAt { get; set; }
        public int NumberOfShifts { get; set; }
        public string CurrentTesterName { set; get; }
        public TimeSpan PeriodPerAppointment { get; set; }
        public MedicalLabDto MapFromModel(MedicalLab model)
        {
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            NumberOfShifts = model.NumberOfShifts;
            PeriodPerAppointment = model.PeriodPerAppointment;
            var now = DateTime.Now.TimeOfDay;
            CurrentTesterName = model.CurrentWorkingTester.Name;

            return this;
        }
    }
}