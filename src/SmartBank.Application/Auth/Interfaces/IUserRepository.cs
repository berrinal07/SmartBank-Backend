using SmartBank.Domain.Entities;

namespace SmartBank.Application.Auth.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(Guid id);

    Task<User> AddAsync(User user);

    Task UpdateAsync(User user);
}