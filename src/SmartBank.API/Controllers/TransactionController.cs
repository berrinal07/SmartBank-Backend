
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBank.Application.Transactions.Interfaces;
using System.Security.Claims;

namespace SmartBank.API.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService
        _transactionService;

    public TransactionController(
        ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyTransactions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(
                userIdClaim,
                out var userId))
        {
            return Unauthorized(new
            {
                message =
                    "Kullanıcı kimliği bulunamadı."
            });
        }

        if (page < 1)
        {
            return BadRequest(new
            {
                message =
                    "Sayfa numarası 1 veya daha büyük olmalıdır."
            });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new
            {
                message =
                    "Sayfa boyutu 1 ile 100 arasında olmalıdır."
            });
        }

        var transactions =
            await _transactionService
                .GetMyTransactionsAsync(
                    userId,
                    page,
                    pageSize);

        return Ok(transactions);
    }
}

