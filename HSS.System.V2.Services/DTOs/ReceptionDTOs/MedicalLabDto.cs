using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class MedicalLabDto : IOutputDto<MedicalLabDto, MedicalLab>
    {
        public string Id { set; get; }
        public TimeSpan StartAt { get; set; }
        public TimeSpan? EndAt { get; set; }
        public int NumberOfShifts { get; set; }
        public string CurrentTesterName { set; get; }
        public TimeSpan PeriodPerAppointment { get; set; }
        public MedicalLabDto MapFromModel(MedicalLab model)
        {
            Id = model.Id;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            NumberOfShifts = model.NumberOfShifts;
            PeriodPerAppointment = model.PeriodPerAppointment;
            var now = HelperDate.GetCurrentDate().TimeOfDay;
            CurrentTesterName = model.CurrentWorkingTester?.Name ?? "غير محدد";

            return this;
        }
    }
}