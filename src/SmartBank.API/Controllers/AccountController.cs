using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBank.Application.Accounts.Interfaces;
using SmartBank.Application.DTOs;
using System.Security.Claims;

namespace SmartBank.API.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<ActionResult<AccountResponse>> Create(
        CreateAccountRequest request)
    {
        try
        {
            var userId = GetUserId();

            var response =
                await _accountService.CreateAsync(
                    userId,
                    request);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountSummaryResponse>>> GetMyAccounts()
    {
        var userId = GetUserId();

        var response =
            await _accountService.GetMyAccountsAsync(userId);

        return Ok(response);
    }

    [HttpGet("{accountId:guid}")]
    public async Task<ActionResult<AccountResponse>> GetById(
        Guid accountId)
    {
        try
        {
            var userId = GetUserId();

            var response =
                await _accountService.GetByIdAsync(
                    userId,
                    accountId);

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