using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.ResultHelpers.Errors;

using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly AppDbContext _context;
        private readonly IAppointmentRepository _appointmentRepository;

        public QueueRepository(AppDbContext context, IAppointmentRepository appointmentRepository)
        {
            _context = context;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Result<TQueue>> GetQueueById<TQueue>(string queueId)
            where TQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<TQueue>
        {
            var queue = await _context.Set<TQueue>()
              .Where(q => q.Id == queueId)
              .Include(q => q.GetInclude())
              .FirstOrDefaultAsync();
            return queue is null ? EntityNotExistsError.Happen<TQueue>(queueId) : queue;
        }
        public async Task<Result<TQueue>> GetQueueByDepartmentId<TQueue>(string departmentId) where TQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<TQueue>
        {
            try
            {
                var entity = await _context.Set<TQueue>()
                                    .AsNoTracking()
                                    .Where(q => q.DepartmentId == departmentId)
                                    .Include(q => q.GetInclude())
                                    .FirstOrDefaultAsync();

                return entity is null
                    ? EntityNotExistsError.Happen<TQueue>(departmentId)
                    : Result.Ok(entity);
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }
        public async Task<Result> CreateQueue(SystemQueue queue)
        {
            await _context.SystemQueues.AddAsync(queue);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        public async Task<Result> DeleteQueue(string queueId)
        {
            var queue = await GetQueue(queueId);
            if (queue is null)
                return EntityNotExistsError.Happen<SystemQueue>(queueId);

            _context.SystemQueues.Remove(queue);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        public async Task<Result> DeleteQueue(SystemQueue model)
        {
            var queue = await GetQueue(model.Id);
            if (queue is null)
                return EntityNotExistsError.Happen<SystemQueue>(model.Id);
            _context.SystemQueues.Remove(queue);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> UpdateQueue(SystemQueue queue)
        {
            try
            {
                _context.SystemQueues.Update(queue);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result> AddAppointmentToQueue<TApp, TQueue>(string appointmentId, string queueId)
            where TApp : Appointment, IAppointmentModel<TQueue> 
            where TQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<TQueue>
        {
            var queueResult = await GetQueueById<TQueue>(queueId);
            if (queueResult.IsFailed)
                return Result.Fail(queueResult.Errors);
            var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<TApp>(appointmentId);
            if (appointmentResult.IsFailed)
                return Result.Fail(appointmentResult.Errors);
            var appointment = appointmentResult.Value;
            var queue = queueResult.Value;
            if (queue.Appointments.Any(a => a.Id == appointmentId))
                return new BadRequestError("This appointment is already in the queue");

            appointment.SetQueue(queue);
            appointment.ActualStartAt = CalculateNextAvailableAppointmentTime(queue, appointment.SchaudleStartAt.Date);
            RecalculateQueueAppointmentTimes(queue);
            return await _appointmentRepository.UpdateAppointmentAsync(appointment)
                        .ThenAsync(() => UpdateQueue(queue));
        }

        public async Task<Result> RemoveAppointmentFromQueue<TApp, TQueue>(string appointmentId)
            where TApp : Appointment, IAppointmentModel<TQueue>
            where TQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<TQueue>
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync<TApp>(appointmentId);
                if (appointment is null)
                    return EntityNotExistsError.Happen<TApp>(appointmentId);
                appointment.Value.SetQueue(null);
                await _appointmentRepository.UpdateAppointmentAsync(appointment.Value);
                var queue = appointment.Value.GetQueue();
                if (queue is not null)
                    RecalculateQueueAppointmentTimes(queue);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        private Task<SystemQueue?> GetQueue(string id)
        {
            return _context.SystemQueues
                .FirstOrDefaultAsync(q => q.Id == id);
        }


        private DateTime CalculateNextAvailableAppointmentTime(SystemQueue q, DateTime? startAt)
        {
            DateTime now = startAt ?? DateTime.UtcNow;

            // Get appointments that already have a StartAt in the future (or now)
            var futureAppointments = (q.Appointments ?? new List<Appointment>())
                .Where(a => a.ActualStartAt.HasValue && a.ActualStartAt.Value >= now)
                .ToList();

            // If there are any appointments scheduled in the future, the next slot is after the latest one.
            if (futureAppointments.Any())
            {
                DateTime latest = futureAppointments.Max(a => a.ActualStartAt ?? DateTime.MaxValue);
                return latest.Add(q.PeriodPerAppointment);
            }

            // Otherwise, no future appointments: schedule for one minute from now.
            return now.AddMinutes(1);
        }

        /// <summary>
        /// Recalculates and reassigns the StartAt times for all appointments in the given queue,
        /// ensuring that no appointment is scheduled in the past. The first available slot is set to one minute from now
        /// if the department's working time has already passed, and subsequent appointments are scheduled at intervals
        /// equal to the queue's PeriodPerAppointment.
        /// </summary>
        /// <param name="q">The queue whose appointments will be rescheduled.</param>
        public static void ReorderQueueAppointments(SystemQueue q)
        {
            DateTime now = DateTime.UtcNow;
            // The first available slot: if now is in the past compared to the scheduled time, we use now+1 minute.
            DateTime nextAvailable = now.AddMinutes(1);

            // Order appointments by their current StartAt if available; otherwise, by their ExpectedTimeForStart.
            var orderedAppointments = (q.Appointments ?? new List<Appointment>())
                .OrderBy(a => a.ActualStartAt ?? a.SchaudleStartAt)
                .ToList();

            // Reassign StartAt times sequentially.
            foreach (var appointment in orderedAppointments)
            {
                // If the current nextAvailable is in the past, adjust to one minute from now.
                if (nextAvailable < now)
                    nextAvailable = now.AddMinutes(1);

                appointment.ActualStartAt = nextAvailable;
                nextAvailable = nextAvailable.Add(q.PeriodPerAppointment);
            }
        }

        /// <summary>
        /// Recalculates and reassigns the StartAt times for all appointments in the specified queue.
        /// The appointments are first ordered by their effective arrival time (using DateTime.UtcNow for early arrivals).
        /// Starting from the department's StartAt time (for today), each appointment is assigned a time slot at an interval equal to the queue's PeriodPerAppointment.
        /// If the calculated slot exceeds the department's EndAt time, the appointment’s StartAt is set to null.
        /// </summary>
        /// <param name="q">The queue whose appointments will be rescheduled. The queue must have its HospitalDepartment set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="q"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the queue's HospitalDepartment is not set.</exception>
        public void RecalculateQueueAppointmentTimes<TQueue>(TQueue q) where TQueue: SystemQueue, IQueueModel
        {
            if (q == null)
                throw new ArgumentNullException(nameof(q));

            DateTime baseTime = DateTime.Today.Add(q.DepartmentStartAt);

            DateTime departmentEndTime = DateTime.Today.Add(q.DepartmentEndAt);

            DateTime GetEffectiveTime(Appointment a) =>
                DateTime.UtcNow < a.SchaudleStartAt ? DateTime.UtcNow : a.SchaudleStartAt;

            var orderedAppointments = (q.Appointments ?? new List<Appointment>())
                                        .OrderBy(a => GetEffectiveTime(a))
                                        .ToList();

            for (int i = 0; i < orderedAppointments.Count; i++)
            {
                DateTime proposedTime = baseTime.Add(q.PeriodPerAppointment * i);
                if (proposedTime <= departmentEndTime)
                    orderedAppointments[i].ActualStartAt = proposedTime;
                else
                    orderedAppointments[i].ActualStartAt = null;
            }
        }

        public Task<(DateTime StartAt, int Index)> GetAppointemntCustomDetails(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
