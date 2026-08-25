using SmartBank.Domain.Entities;

namespace SmartBank.Application.Auth.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
}