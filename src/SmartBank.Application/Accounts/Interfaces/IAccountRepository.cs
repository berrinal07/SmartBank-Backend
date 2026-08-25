using SmartBank.Domain.Entities;

namespace SmartBank.Application.Accounts.Interfaces;

public interface IAccountRepository
{
    Task AddAsync(Account account);

    Task<List<Account>> GetByUserIdAsync(Guid userId);

    Task<Account?> GetByIdAsync(Guid id, Guid userId);

    Task<bool> ExistsByAccountNumberAsync(string accountNumber);

    Task SaveChangesAsync();
}