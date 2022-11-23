using ProjectHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHub.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _appDbContext;

        public NotificationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddNotification(Notification notification)
        {
            _appDbContext.Notifications.Add(notification);
            _appDbContext.SaveChanges();
        }

        public void DeleteAllProjectsNotifications(int projectId)
        {
            var notifications = _appDbContext.Notifications.Where(n => n.ProjectId == projectId);

            foreach (var notification in notifications.ToList())
            {
                _appDbContext.Notifications.Remove(notification);
                _appDbContext.SaveChanges();
            }
        }

        public void DeleteNotification(int notificationId)
        {
            Notification notification = _appDbContext.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
            _appDbContext.Notifications.Remove(notification);
            _appDbContext.SaveChanges();
        }

        public void EditNotification(Notification notification)
        {
            _appDbContext.Notifications.Update(notification);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Notification> GetAllUserNotifications(string userId)
        {
            return _appDbContext.Notifications.Where(n => n.UserId == userId);
        }

        public IEnumerable<Notification> GetAllUserProjectNotifications(string userId, int projectId)
        {
            return _appDbContext.Notifications.Where(n => n.UserId == userId && n.ProjectId == projectId);
        }

        public Notification GetNotificationById(int notificationId)
        {
            return _appDbContext.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
        }
    }
}
