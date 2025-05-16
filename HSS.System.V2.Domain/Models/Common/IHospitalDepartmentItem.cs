namespace HSS.System.V2.Domain.Models.Common;

public interface IHospitalDepartmentItem
{
    TimeSpan StartAt { get; }
    TimeSpan EndAt { get; }
    TimeSpan PeriodPerAppointment { get; }
}
