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
            where TQueue : SystemQueue
        {
            var queue = await _context.Set<TQueue>()
              .Where(q => q.Id == queueId)
              .FirstOrDefaultAsync();
            return queue is null ? EntityNotExistsError.Happen<TQueue>(queueId) : queue;
        }
        public async Task<Result<TQueue>> GetQueueByDepartmentId<TQueue>(string departmentId) where TQueue : SystemQueue
        {
            try
            {
                var entity = await _context.Set<TQueue>()
                                    .AsNoTracking()
                                    .Where(q => q.ClinicId == departmentId || q.RadiologyCeneterId == departmentId || q.MedicalLabId == departmentId)
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
            where TQueue : SystemQueue
        {
            #region Logic 
            // if the queue is empty,
            //    if the current date before the department start at 
            //        appointment's actual start at is at department start at
            //    else, appointment's actual start at is now
            // else
            //    if the appointment's schaduled start at is between or before the dates of actual start at in the current appointments in the queue
            //      append it in the right place based on all appointment's schudled start at and shift the after appointments to the correct times
            //    else (the appointment's scheduled start at after all the appointments in the queue)
            //      if the last appointment's actual start at + the schduled period per appointment is bigger then or equal current date
            //        make the appointment's actual start at, at last appointment's start at + the schduled period per appointment
            //      else
            //        make the appointment's start at, now
            #endregion

            // 1. Fetch the queue
            var queueResult = await GetQueueById<TQueue>(queueId);
            if (queueResult.IsFailed)
                return Result.Fail(queueResult.Errors);

            // 2. Fetch the appointment
            var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<TApp>(appointmentId);
            if (appointmentResult.IsFailed)
                return Result.Fail(appointmentResult.Errors);

            var queue = queueResult.Value;
            var appointment = appointmentResult.Value;
            appointment.State = Domain.Enums.AppointmentState.InQueue;

            // 3. Check if already in queue
            if (queue.Appointments.Any(a => a.Id == appointmentId))
                return new BadRequestError("This appointment is already in the queue");

            //------------------------------------------------------------
            // 4. Compute the new appointment’s ActualStartAt “by hand”:
            //    – if the queue is empty ⇒ either department‐start‐at or now
            //    – else ⇒ either “insert in middle” or “append at end”
            //------------------------------------------------------------

            // We need a mutable list so we can insert in “the right place”:
            var sortedList = queue.Appointments
                                  .OrderBy(a => a.ActualStartAt ?? a.SchaudleStartAt)
                                  .ToList();

            DateTime now = HelperDate.GetCurrentDate();
            TimeSpan period = queue.PeriodPerAppointment;

            if (!sortedList.Any())
            {
                // Queue is empty.
                // Build today’s department start‐at as a DateTime:
                // ‣ e.g. if DepartmentStartAt = 08:30, then deptStartDt = Today @ 08:30
                DateTime today = DateTime.Today;
                DateTime deptStartDt = today.Add(queue.DepartmentStartAt);

                if (now < deptStartDt)
                {
                    appointment.ActualStartAt = deptStartDt;
                }
                else
                {
                    appointment.ActualStartAt = now;
                }

                // Add to the (empty) queue
                sortedList.Add(appointment);
            }
            else
            {
                // There are already some appointments in queue.
                // Find if the new appointment’s ScheduledStartAt should go “in the middle”:
                int insertIndex = sortedList
                    .FindIndex(existing => appointment.SchaudleStartAt <= (existing.ActualStartAt ?? existing.SchaudleStartAt));

                if (insertIndex >= 0)
                {
                    // (A) “Insert in middle” case:
                    //    – We want the new appointment to begin at its ScheduledStartAt,
                    //      but if that time conflicts with the previous appointment’s actual‐slot,
                    //      we “push” it to (previous.ActualStartAt + PeriodPerAppointment).
                    DateTime prevSlotStart;
                    if (insertIndex == 0)
                    {
                        // Inserting before everything ⇒ compare against department start‐at
                        DateTime today = DateTime.Today;
                        DateTime firstDeptStartDt = today.Add(queue.DepartmentStartAt);
                        prevSlotStart = firstDeptStartDt.Add(-period);
                        // so that max(prevSlotStart + period, scheduled) works out
                    }
                    else
                    {
                        prevSlotStart = sortedList[insertIndex - 1].ActualStartAt
                                        ?? sortedList[insertIndex - 1].SchaudleStartAt;
                    }

                    // Desired slot = max(SchaudleStartAt, prevSlotStart + period, now) 
                    DateTime earliestBasedOnPrev = prevSlotStart.Add(period);
                    DateTime desired = appointment.SchaudleStartAt > earliestBasedOnPrev
                                       ? appointment.SchaudleStartAt
                                       : earliestBasedOnPrev;

                    // But if desired < now, we can still start it at “now”
                    appointment.ActualStartAt = (desired > now) ? desired : now;

                    sortedList.Insert(insertIndex, appointment);
                }
                else
                {
                    // (B) “Append at end” case: scheduled after every existing actual‐start
                    var last = sortedList.Last();
                    DateTime lastSlotStart = last.ActualStartAt ?? last.SchaudleStartAt;
                    DateTime nextPossible = lastSlotStart.Add(period);

                    if (nextPossible >= now)
                        appointment.ActualStartAt = nextPossible;
                    else
                        appointment.ActualStartAt = now;

                    sortedList.Add(appointment);
                }
            }

            //------------------------------------------------------------
            // 5. Now that we have “sortedList” with the new appointment inserted,
            //    we need to re‐assign ActualStartAt for every appointment in order,
            //    using the first appointment’s ActualStartAt as a “seed” and then
            //    doing +PeriodPerAppointment for each subsequent one.
            //------------------------------------------------------------
            if (sortedList.Count > 0)
            {
                // Take the first appointment’s computed ActualStartAt (as‐is) and build from there:
                DateTime seed = sortedList[0].ActualStartAt!.Value;

                // We will rewrite each ActualStartAt in ascending index order,
                // so that there are no overlaps and each slot is exactly “period” apart.
                sortedList[0].ActualStartAt = seed;

                for (int i = 1; i < sortedList.Count; i++)
                {
                    sortedList[i].ActualStartAt = sortedList[i - 1].ActualStartAt!.Value.Add(period);
                }
            }

            //------------------------------------------------------------
            // 6. Replace the queue’s Appointment‐collection with our newly‐ordered list.
            //    (Again, adjust this to whatever type your TQueue actually uses.)
            //------------------------------------------------------------
            // If TQueue.Appointments is an ICollection<Appointment> (e.g. List<Appointment>), do:
            //    queue.Appointments.Clear();
            //    foreach (var appt in sortedList) queue.Appointments.Add(appt);
            //
            // If it’s some other EF‐managed navigation, adapt accordingly.
            //
            // Here is one “generic” way if you know it’s a List<Appointment>:
            if (queue.Appointments is IList<Appointment> listBacking)
            {
                listBacking.Clear();
                foreach (var appt in sortedList)
                    listBacking.Add(appt);
            }
            else
            {
                // If it is IReadOnly or something else, you might need to use
                // EF change‐tracking or a different approach. 
                // For simplicity, let’s assume it’s convertible to a List<Appointment> and re‐assignable:
                var newCollection = new List<Appointment>(sortedList);
                // (Replace the navigation property—only works if your EF model allows set; please adjust.)
                typeof(TQueue)
                    .GetProperty(nameof(SystemQueue.Appointments))!
                    .SetValue(queue, newCollection);
            }

            //------------------------------------------------------------
            // 7. Finally, persist the updated queue back to DB
            //------------------------------------------------------------
            return await UpdateQueue(queue);
        }


        public async Task<Result> RemoveAppointmentFromQueue<TApp, TQueue>(string appointmentId)
              where TApp : Appointment, IAppointmentModel<TQueue>
              where TQueue : SystemQueue
        {
            try
            {

                #region Logic
                // if the appointment in the queue 
                //   remove the appointment from queue ( delete the queue object from it not premently remove) and then shift the actual time for all the next appointments
                #endregion


                // 1. Fetch the appointment (ensure it exists)
                var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<TApp>(appointmentId);
                if (appointmentResult.IsFailed || appointmentResult.Value == null)
                    return EntityNotExistsError.Happen<TApp>(appointmentId);

                var appointment = appointmentResult.Value;

                // 2. Determine which queue it belongs to (if any)
                var queue = appointment.GetQueue();
                if (queue == null)
                    return new BadRequestError("This appointment is not in any queue.");

                // 3. Before detaching, capture the appointment’s old ActualStartAt (or Scheduled if Actual is null)
                DateTime removedStart = appointment.ActualStartAt ?? appointment.SchaudleStartAt;

                // 4. Detach the appointment from its queue
                appointment.SetQueue(null);
                appointment.State = Domain.Enums.AppointmentState.NotStarted;
                appointment.ActualStartAt = null;
                await _appointmentRepository.UpdateAppointmentAsync(appointment);

                // 5. Build a sorted list of all remaining appointments in that queue,
                //    ordered by (ActualStartAt || ScheduledStartAt). Exclude the removed one.
                var remaining = queue.Appointments
                                     .Where(a => a.Id != appointmentId)
                                     .OrderBy(a => a.ActualStartAt ?? a.SchaudleStartAt)
                                     .ToList();

                if (!remaining.Any())
                {
                    // Nothing left in queue—done.
                    return Result.Ok();
                }

                TimeSpan period = queue.PeriodPerAppointment;
                DateTime now = HelperDate.GetCurrentDate();

                // 6. Compute today’s “department start” as a DateTime
                //    (e.g. if DepartmentStartAt is 08:30, then deptStartDt = Today @ 08:30)
                DateTime deptStartDt = DateTime.Today.Add(queue.DepartmentStartAt);

                // 7. Figure out how many appointments in the *old* queue came before the removed one
                //    (so we know where to begin shifting).
                int countBefore = queue.Appointments.Count(a =>
                    a.Id != appointmentId &&
                    ((a.ActualStartAt ?? a.SchaudleStartAt) < removedStart));

                if (countBefore == 0)
                {
                    // 7a. Removed appointment was (or tied for) first in the old ordering.
                    //     ⇒ “Open up” the first slot. Move the new first appointment to max(deptStart, now).
                    DateTime newFirstStart = (now < deptStartDt) ? deptStartDt : now;
                    remaining[0].ActualStartAt = newFirstStart;

                    //     Then push everyone else by exactly one period each.
                    for (int i = 1; i < remaining.Count; i++)
                    {
                        remaining[i].ActualStartAt =
                            remaining[i - 1].ActualStartAt.Value.Add(period);
                    }
                }
                else
                {
                    // 7b. The removed appointment was not first. That means the first (countBefore) slots
                    //     remain exactly as they were. Only start shifting from the next index onward.
                    //
                    //     For i == countBefore, we take the (countBefore - 1)th appointment’s ActualStartAt
                    //     (which did not change) and set this slot = previous + period.
                    for (int i = countBefore; i < remaining.Count; i++)
                    {
                        DateTime prevStart =
                            (i == 0)
                                // (This branch never runs because countBefore > 0 ⇒ i >= 1)
                                ? ((now < deptStartDt) ? deptStartDt : now)
                                : remaining[i - 1].ActualStartAt!.Value;

                        remaining[i].ActualStartAt = prevStart.Add(period);
                    }
                }

                // 8. Persist all updated appointment times back to the database
                foreach (var appt in remaining)
                {
                    await _appointmentRepository.UpdateAppointmentAsync(appt);
                }

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
        /// <param name="queue">The queue whose appointments will be rescheduled. The queue must have its HospitalDepartment set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="queue"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the queue's HospitalDepartment is not set.</exception>
        public void RecalculateQueueAppointmentTimes<TQueue>(TQueue queue) where TQueue: SystemQueue
        {
            if (queue == null)
                return;

            DateTime baseTime = DateTime.Today.Add(queue.DepartmentStartAt);

            DateTime departmentEndTime = DateTime.Today.Add(queue.DepartmentEndAt);

            DateTime GetEffectiveTime(Appointment a) =>
                DateTime.UtcNow < a.SchaudleStartAt ? DateTime.UtcNow : a.SchaudleStartAt;

            var orderedAppointments = (queue.Appointments ?? new List<Appointment>())
                                        .OrderBy(a => GetEffectiveTime(a))
                                        .ToList();

            for (int i = 0; i < orderedAppointments.Count; i++)
            {
                DateTime proposedTime = baseTime.Add(queue.PeriodPerAppointment * i);
                if (proposedTime <= departmentEndTime)
                    orderedAppointments[i].ActualStartAt = proposedTime;
                else
                    orderedAppointments[i].ActualStartAt = null;
            }
        }

        public void RecalculateQueueAppointmentTimes<TQueue>(string queueId) where TQueue : SystemQueue
        {
            var queue = _context.Set<TQueue>().FirstOrDefault(q => q.Id == queueId);
            RecalculateQueueAppointmentTimes<TQueue>(queue);
        }


        public Task<(DateTime StartAt, int Index)> GetAppointemntCustomDetails(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
