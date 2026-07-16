using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Services
{
    public class NotificationService
    {
        private readonly SchoolContext _context;

        public NotificationService(SchoolContext context)
        {
            _context = context;
        }

        public void SendNotification(string entityType, string entityId, EntityOperation operation, string userName = null)
        {
            SendNotification(entityType, entityId, null, operation, userName);
        }

        public void SendNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation, string userName = null)
        {
            var notification = new Notification
            {
                EntityType = entityType,
                EntityId = entityId,
                Operation = operation.ToString(),
                Message = GenerateMessage(entityType, entityId, entityDisplayName, operation),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userName ?? "System",
                IsRead = false
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }

        public List<Notification> GetUnreadNotifications(int maxCount = 10)
        {
            return _context.Notifications
                .AsNoTracking()
                .Where(n => !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Take(maxCount)
                .ToList();
        }

        public void MarkAsRead(int notificationId)
        {
            var notification = _context.Notifications.SingleOrDefault(n => n.Id == notificationId);
            if (notification == null)
            {
                return;
            }

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            _context.SaveChanges();
        }

        private string GenerateMessage(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
        {
            var displayText = !string.IsNullOrWhiteSpace(entityDisplayName)
                ? $"{entityType} '{entityDisplayName}'"
                : $"{entityType} (ID: {entityId})";

            return operation switch
            {
                EntityOperation.CREATE => $"New {displayText} has been created",
                EntityOperation.UPDATE => $"{displayText} has been updated",
                EntityOperation.DELETE => $"{displayText} has been deleted",
                _ => $"{displayText} operation: {operation}"
            };
        }
    }
}
