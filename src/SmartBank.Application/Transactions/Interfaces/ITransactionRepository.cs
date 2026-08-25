
using SmartBank.Domain.Entities;

namespace SmartBank.Application.Transactions.Interfaces;

public interface ITransactionRepository
{
    Task<(List<BankTransaction> Items, int TotalCount)>
        GetByUserIdAsync(
            Guid userId,
            int page,
            int pageSize);
}

