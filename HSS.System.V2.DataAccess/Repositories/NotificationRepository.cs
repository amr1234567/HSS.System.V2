using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Models.Notifications;

using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.DataAccess.Repositories
{
    internal class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<AppNotification>>> GetAllNotificationsForUser(string userId)
        {
            return await _context.Notifications
                .Where(n => n.PatientId == userId)
                .ToListAsync();
        }

        public async Task<Result<IEnumerable<AppNotification>>> GetAllNotificationsForUser(string userId, bool isSeen)
        {
            return await _context.Notifications
               .Where(n => n.PatientId == userId && n.Seen == isSeen)
               .ToListAsync();
        }

        public async Task<Result> MarkNotificationAsSeen(string notificationId)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
            if (notification == null)
            {
                return Result.Fail("Notification not found");
            }
            notification.Seen = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<AppNotification?>> GetNotificationById(string notificationId)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);
        }

        public async Task<Result> CreateNotification(AppNotification model)
        {
            await _context.Notifications.AddAsync(model);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
