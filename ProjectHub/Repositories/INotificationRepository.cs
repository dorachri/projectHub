using ProjectHub.Models;
using System.Collections.Generic;

namespace ProjectHub.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetAllUserNotifications(string userId);

        IEnumerable<Notification> GetAllUserProjectNotifications(string userId, int projectId);

        void DeleteAllProjectsNotifications(int projectId);
        
        Notification GetNotificationById(int notificationId);

        void AddNotification(Notification notification);

        void EditNotification(Notification notification);

        void DeleteNotification(int notificationId);
    }
}
