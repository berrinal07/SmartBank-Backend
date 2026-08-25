
using SmartBank.Application.DTOs;
using SmartBank.Application.Transactions.Interfaces;

namespace SmartBank.Application.Transactions.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository
        _transactionRepository;

    public TransactionService(
        ITransactionRepository transactionRepository)
    {
        _transactionRepository =
            transactionRepository;
    }

    public async Task<TransactionListResponse>
        GetMyTransactionsAsync(
            Guid userId,
            int page,
            int pageSize)
    {
        var result =
            await _transactionRepository
                .GetByUserIdAsync(
                    userId,
                    page,
                    pageSize);

        var totalPages =
            (int)Math.Ceiling(
                result.TotalCount /
                (double)pageSize);

        return new TransactionListResponse
        {
            Items = result.Items
                .Select(transaction =>
                    new TransactionResponse
                    {
                        Id = transaction.Id,
                        AccountId =
                            transaction.AccountId,
                        Amount =
                            transaction.Amount,
                        TransactionType =
                            transaction.TransactionType,
                        Description =
                            transaction.Description,
                        RelatedAccountId =
                            transaction.RelatedAccountId,
                        CreatedAt =
                            transaction.CreatedAt
                    })
                .ToList(),

            Page = page,
            PageSize = pageSize,
            TotalCount = result.TotalCount,
            TotalPages = totalPages
        };
    }
}

