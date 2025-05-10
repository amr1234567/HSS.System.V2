using FluentResults;

using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Queues;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IQueueRepository
    {
        Task<Result<TQueue>> GetQueueById<TQueue>(string queueId) where TQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<TQueue>;
        Task<Result<TQueue>> GetQueueByDepartmentId<TQueue>(string departmentId) where TQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<TQueue>;
        Task<Result> CreateQueue(SystemQueue queue);
        Task<Result> DeleteQueue(string queueId);
        Task<Result> DeleteQueue(SystemQueue model);
        Task<Result> AddAppointmentToQueue<TApp, TQueue>(string appointmentId, string queueId) where TApp : Appointment, IAppointmentModel<TQueue> where TQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<TQueue>;
        Task<Result> RemoveAppointmentFromQueue<TApp, TQueue>(string appointmentId)
            where TApp : Appointment, IAppointmentModel<TQueue>
            where TQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<TQueue>;
        void RecalculateQueueAppointmentTimes<TQueue>(TQueue q) where TQueue : SystemQueue, IQueueModel;

        Task<(DateTime StartAt, int Index)> GetAppointemntCustomDetails(Appointment appointment);
    }
}
