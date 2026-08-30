using SmartBank.Domain.Entities;

namespace SmartBank.Application.Notifications.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetByUserIdAsync(Guid userId);

    Task<int> GetUnreadCountAsync(Guid userId);

    Task<Notification?> GetByIdAsync(Guid id);

    Task AddAsync(Notification notification);

    Task SaveChangesAsync();
}