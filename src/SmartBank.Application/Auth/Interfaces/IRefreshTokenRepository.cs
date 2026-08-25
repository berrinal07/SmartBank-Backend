using SmartBank.Domain.Entities;

namespace SmartBank.Application.Auth.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);

    Task<RefreshToken?> GetByTokenAsync(string token);

    Task SaveChangesAsync();

    Task RevokeAsync(RefreshToken refreshToken);
}