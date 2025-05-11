using FluentResults;

using HSS.System.V2.Domain.Models.Notifications;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface INotificationRepository
    {
        Task<Result> CreateNotification(AppNotification model);
        Task<Result<IEnumerable<AppNotification>>> GetAllNotificationsForUser(string userId);
        Task<Result<IEnumerable<AppNotification>>> GetAllNotificationsForUser(string userId, bool isSeen);

        Task<Result> MarkNotificationAsSeen(string notificationId);
        Task<Result<AppNotification?>> GetNotificationById(string notificationId);
    }
}
