
using SmartBank.Application.DTOs;

namespace SmartBank.Application.Transactions.Interfaces;

public interface ITransactionService
{
    Task<TransactionListResponse> GetMyTransactionsAsync(
        Guid userId,
        int page,
        int pageSize);
}

