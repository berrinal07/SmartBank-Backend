using Microsoft.EntityFrameworkCore;
using SmartBank.Application.Accounts.Interfaces;
using SmartBank.Domain.Entities;
using SmartBank.Infrastructure.Data;

namespace SmartBank.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public async Task<List<Account>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Accounts
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(
        Guid id,
        Guid userId)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.UserId == userId);
    }

    public async Task<bool> ExistsByAccountNumberAsync(
        string accountNumber)
    {
        return await _context.Accounts
            .AnyAsync(a =>
                a.AccountNumber == accountNumber);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}