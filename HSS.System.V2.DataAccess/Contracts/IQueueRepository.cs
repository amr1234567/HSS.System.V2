using FluentResults;

using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Queues;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IQueueRepository
    {
        Task<Result<TQueue>> GetQueueById<TQueue>(string queueId) where TQueue : SystemQueue;
        Task<Result<TQueue>> GetQueueByDepartmentId<TQueue>(string departmentId) where TQueue : SystemQueue;
        Task<Result> CreateQueue(SystemQueue queue);
        Task<Result> DeleteQueue(string queueId);
        Task<Result> DeleteQueue(SystemQueue model);
        Task<Result> AddAppointmentToQueue<TApp, TQueue>(string appointmentId, string queueId) where TApp : Appointment, IAppointmentModel<TQueue> where TQueue : SystemQueue;
        Task<Result> RemoveAppointmentFromQueue<TApp, TQueue>(string appointmentId)
            where TApp : Appointment, IAppointmentModel<TQueue>
            where TQueue : SystemQueue;
        void RecalculateQueueAppointmentTimes<TQueue>(TQueue q) where TQueue : SystemQueue;
        void RecalculateQueueAppointmentTimes<TQueue>(string queueId) where TQueue : SystemQueue;
        Task<(DateTime StartAt, int Index, string Name, string Type)> GetAppointemntCustomDetails(Appointment appointment);
    }
}
