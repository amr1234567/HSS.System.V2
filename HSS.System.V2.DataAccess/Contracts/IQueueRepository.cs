using FluentResults;

using HSS.System.V2.Domain.Queues;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IQueueRepository
    {
        Task<Result<TQueue>> GetQueueById<TQueue>(string queueId) where TQueue : SystemQueue;
        Task<Result<TQueue>> GetQueueByDepartmentId<TQueue>(string departmentId) where TQueue : SystemQueue;
        Task<Result> CreateQueue(SystemQueue queue);
        Task<Result> DeleteQueue(string queueId);
        Task<Result> DeleteQueue(SystemQueue model);
        Task<Result<SystemQueue>> GetQueueByRoomId(string roomId);
        Task<Result> AddAppointmentToQueue(string appointmentId, string queueId);
        Task<Result> RemoveAppointmentFromQueue(string appointmentId);
        Task<Result> ReSchaduleTheQueue(string queueId);

    }
}
