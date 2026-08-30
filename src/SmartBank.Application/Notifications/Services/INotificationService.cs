using SmartBank.Application.DTOs;

namespace SmartBank.Application.Notifications.Services;

public interface INotificationService
{
    Task<List<NotificationDto>> GetMyNotificationsAsync(Guid userId);

    Task<int> GetUnreadCountAsync(Guid userId);

    Task MarkAsReadAsync(Guid notificationId, Guid userId);
}