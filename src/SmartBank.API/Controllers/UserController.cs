using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBank.Application.Auth.Interfaces;
using SmartBank.Application.DTOs;

namespace SmartBank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileResponse>> GetProfile()
    {
        var userId = GetUserId();

        try
        {
            var response =
                await _userService.GetProfileAsync(userId);

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfile(
        UpdateUserProfileRequest request)
    {
        var userId = GetUserId();

        try
        {
            var response =
                await _userService.UpdateProfileAsync(
                    userId,
                    request);

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    [HttpPut("password")]
public async Task<IActionResult> UpdatePassword(
    UpdatePasswordRequest request)
{
    var userId = GetUserId();

    try
    {
        await _userService.UpdatePasswordAsync(
            userId,
            request);

        return Ok(new
        {
            message = "Şifre başarıyla güncellendi."
        });
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new
        {
            message = ex.Message
        });
    }
    catch (UnauthorizedAccessException ex)
    {
        return Unauthorized(new
        {
            message = ex.Message
        });
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(new
        {
            message = ex.Message
        });
    }
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