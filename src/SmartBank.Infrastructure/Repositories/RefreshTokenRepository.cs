using Microsoft.EntityFrameworkCore;
using SmartBank.Application.Auth.Interfaces;
using SmartBank.Domain.Entities;
using SmartBank.Infrastructure.Data;

namespace SmartBank.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task RevokeAsync(RefreshToken refreshToken)
{
    refreshToken.RevokedAt = DateTime.UtcNow;

    _context.RefreshTokens.Update(refreshToken);

    await Task.CompletedTask;
}
}