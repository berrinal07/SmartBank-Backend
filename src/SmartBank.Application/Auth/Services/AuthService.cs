using SmartBank.Application.Auth.Interfaces;
using SmartBank.Application.DTOs;
using SmartBank.Domain.Entities;
using System.Security.Cryptography;

namespace SmartBank.Application.Auth.Services;

public class AuthService : IAuthService
{
   private readonly IUserRepository _userRepository;
private readonly IPasswordHasher _passwordHasher;
private readonly IJwtService _jwtService;
private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IRefreshTokenRepository refreshTokenRepository)
{
    _userRepository = userRepository;
    _passwordHasher = passwordHasher;
    _jwtService = jwtService;
    _refreshTokenRepository = refreshTokenRepository;
}

    public async Task<RegisterResponse> RegisterAsync(
        RegisterRequest request)
    {
        var existingUser = await _userRepository
            .GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                "Bu email adresi zaten kayıtlı.");
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user);

        return new RegisterResponse
        {
            Id = createdUser.Id,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
            Email = createdUser.Email
        };
    }


public async Task<LoginResponse> LoginAsync(LoginRequest request)
{
    var user = await _userRepository.GetByEmailAsync(request.Email);

    if (user is null)
    {
        throw new UnauthorizedAccessException(
            "Email veya şifre hatalı.");
    }

    var passwordValid = _passwordHasher.Verify(
        request.Password,
        user.PasswordHash);

    if (!passwordValid)
    {
        throw new UnauthorizedAccessException(
            "Email veya şifre hatalı.");
    }

    var accessToken = _jwtService.GenerateAccessToken(user);

    var refreshToken = new RefreshToken
    {
        Id = Guid.NewGuid(),
        UserId = user.Id,
        Token = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64)),
        CreatedAt = DateTime.UtcNow,
        ExpiresAt = DateTime.UtcNow.AddDays(30)
    };

    await _refreshTokenRepository.AddAsync(refreshToken);
    await _refreshTokenRepository.SaveChangesAsync();

    return new LoginResponse
    {
        AccessToken = accessToken,
        RefreshToken = refreshToken.Token,
        ExpiresAt = DateTime.UtcNow.AddMinutes(15)
    };
}


public async Task<LoginResponse> RefreshTokenAsync(
    RefreshTokenRequest request)
{
    var refreshToken =
        await _refreshTokenRepository.GetByTokenAsync(
            request.RefreshToken);

    if (refreshToken is null)
    {
        throw new UnauthorizedAccessException(
            "Geçersiz refresh token.");
    }

    if (refreshToken.RevokedAt is not null)
    {
        throw new UnauthorizedAccessException(
            "Refresh token iptal edilmiş.");
    }

    if (refreshToken.ExpiresAt <= DateTime.UtcNow)
    {
        throw new UnauthorizedAccessException(
            "Refresh token süresi dolmuş.");
    }

    var user = refreshToken.User;

    await _refreshTokenRepository.RevokeAsync(refreshToken);

    var newAccessToken =
        _jwtService.GenerateAccessToken(user);

    var newRefreshToken = new RefreshToken
    {
        Id = Guid.NewGuid(),
        UserId = user.Id,
        Token = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64)),
        CreatedAt = DateTime.UtcNow,
        ExpiresAt = DateTime.UtcNow.AddDays(30)
    };

    await _refreshTokenRepository.AddAsync(newRefreshToken);

    await _refreshTokenRepository.SaveChangesAsync();

    return new LoginResponse
    {
        AccessToken = newAccessToken,
        RefreshToken = newRefreshToken.Token,
        ExpiresAt = DateTime.UtcNow.AddMinutes(15)
    };
}

}