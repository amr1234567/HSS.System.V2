using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    #region DTO and Input Models

    /// <summary>
    /// Data transfer object representing a clinic.
    /// </summary>
    public class ClinicDto : IOutputDto<ClinicDto, Clinic>
    {
        public string ClinicId { set; get; }
        public string?  CurrentDoctor        { get; set; } = "غير معروف";
        public string   QueueId              { get; set; }
        public TimeSpan PeriodPerAppointment { get; set; }
        public string   SpecializationName   { get; set; }
        public string   SpecializationId     { get; set; }
        public string ClinicName { set; get; }

        public ClinicDto MapFromModel(Clinic model)
        {
            ClinicName = model.Name;
            ClinicId = model.Id;
            QueueId = model.QueueId;
            PeriodPerAppointment = model.PeriodPerAppointment;
            SpecializationId = model.SpecializationId;
            SpecializationName = model.Specialization?.Name ?? "غير معروف";
            CurrentDoctor = model.CurrentWorkingDoctor?.Name ?? "لا يوجد حاليا";

            return this;
        }
    }

    #endregion
}
