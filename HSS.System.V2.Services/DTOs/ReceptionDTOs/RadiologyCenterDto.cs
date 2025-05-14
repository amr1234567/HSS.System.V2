using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    #region DTO and Input Models

    /// <summary>
    /// Data transfer object representing a radiology center.
    /// </summary>
    public class RadiologyCenterDto : IOutputDto<RadiologyCenterDto, RadiologyCenter>
    {
        public TimeSpan StartAt { get; set; }
        public TimeSpan? EndAt { get; set; }
        public int NumberOfShifts { get; set; }
        public string CurrentTesterName { set; get; }
        public TimeSpan PeriodPerAppointment { get; set; }


        public RadiologyCenterDto MapFromModel(RadiologyCenter model)
        {
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            NumberOfShifts = model.NumberOfShifts;
            PeriodPerAppointment = model.PeriodPerAppointment;
            CurrentTesterName = model.CurrentWorkingTester.Name;

            return this;
        }
    }

    #endregion
}
