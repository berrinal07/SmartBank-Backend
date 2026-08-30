using Microsoft.EntityFrameworkCore;
using SmartBank.Application.Notifications.Interfaces;
using SmartBank.Domain.Entities;
using SmartBank.Infrastructure.Data;

namespace SmartBank.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Notification>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }

    public async Task<Notification?> GetByIdAsync(Guid id)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task AddAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}