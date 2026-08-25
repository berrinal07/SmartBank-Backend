using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBank.Application.Transfers;
using SmartBank.Application.Transfers.Interfaces;
using System.Security.Claims;

namespace SmartBank.API.Controllers;

[ApiController]
[Route("api/transfers")]
[Authorize]
public class TransferController : ControllerBase
{
    private readonly ITransferService _transferService;

    public TransferController(
        ITransferService transferService)
    {
        _transferService = transferService;
    }

    [HttpPost]
    public async Task<IActionResult> Transfer(
        TransferRequest request)
    {
        try
        {
            var userId = GetUserId();

            await _transferService.TransferAsync(
                userId,
                request);

            return Ok(new
            {
                message = "Transfer başarıyla gerçekleştirildi."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
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
                "Kullanıcı kimliği bulunamadı.");
        }

        return userId;
    }
}