using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBank.Application.Notifications.Services;

namespace SmartBank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(
        INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotifications()
    {
        var userId = GetUserId();

        var notifications =
            await _notificationService
                .GetMyNotificationsAsync(userId);

        return Ok(notifications);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = GetUserId();

        var count =
            await _notificationService
                .GetUnreadCountAsync(userId);

        return Ok(new { count });
    }

    [HttpPatch("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(
        Guid notificationId)
    {
        var userId = GetUserId();

        await _notificationService
            .MarkAsReadAsync(notificationId, userId);

        return NoContent();
    }

    private Guid GetUserId()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException(
                "Geçersiz kullanıcı kimliği.");
        }

        return userId;
    }
}