using SmartBank.Application.DTOs;
using SmartBank.Application.Notifications.Interfaces;

namespace SmartBank.Application.Notifications.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(
        INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<List<NotificationDto>> GetMyNotificationsAsync(
        Guid userId)
    {
        var notifications =
            await _notificationRepository.GetByUserIdAsync(userId);

        return notifications.Select(n => new NotificationDto
        {
            Id = n.Id,
            Title = n.Title,
            Message = n.Message,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        }).ToList();
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _notificationRepository
            .GetUnreadCountAsync(userId);
    }

    public async Task MarkAsReadAsync(
        Guid notificationId,
        Guid userId)
    {
        var notification =
            await _notificationRepository.GetByIdAsync(notificationId);

        if (notification == null)
            throw new KeyNotFoundException(
                "Bildirim bulunamadı.");

        if (notification.UserId != userId)
            throw new UnauthorizedAccessException(
                "Bu bildirime erişim yetkiniz yok.");

        if (!notification.IsRead)
        {
            notification.IsRead = true;

            await _notificationRepository.SaveChangesAsync();
        }
    }
}