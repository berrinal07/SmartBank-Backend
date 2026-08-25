using SmartBank.Domain.Entities;

namespace SmartBank.Application.Auth.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User> AddAsync(User user);
}