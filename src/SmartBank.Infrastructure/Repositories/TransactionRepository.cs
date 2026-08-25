
using Microsoft.EntityFrameworkCore;
using SmartBank.Application.Transactions.Interfaces;
using SmartBank.Domain.Entities;
using SmartBank.Infrastructure.Data;

namespace SmartBank.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<BankTransaction> Items, int TotalCount)>
        GetByUserIdAsync(
            Guid userId,
            int page,
            int pageSize)
    {
        var query = _context.Transactions
            .Where(t => t.Account.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .Include(t => t.Account)
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}

