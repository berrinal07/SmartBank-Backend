using SmartBank.Application.DTOs;

namespace SmartBank.Application.Auth.Interfaces;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);

    Task<LoginResponse> LoginAsync(LoginRequest request);

    Task<LoginResponse> RefreshTokenAsync(
    RefreshTokenRequest request);
}