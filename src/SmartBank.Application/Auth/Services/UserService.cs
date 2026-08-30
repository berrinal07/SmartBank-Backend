using SmartBank.Application.Auth.Interfaces;
using SmartBank.Application.DTOs;

namespace SmartBank.Application.Auth.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserProfileResponse> GetProfileAsync(
        Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "Kullanıcı bulunamadı.");
        }

        return new UserProfileResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<UserProfileResponse> UpdateProfileAsync(
        Guid userId,
        UpdateUserProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "Kullanıcı bulunamadı.");
        }

        var existingUser =
            await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser is not null &&
            existingUser.Id != userId)
        {
            throw new InvalidOperationException(
                "Bu email adresi zaten kullanılıyor.");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return new UserProfileResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task UpdatePasswordAsync(
    Guid userId,
    UpdatePasswordRequest request)
{
    var user = await _userRepository.GetByIdAsync(userId);

    if (user is null)
    {
        throw new KeyNotFoundException(
            "Kullanıcı bulunamadı.");
    }

    var currentPasswordValid =
        _passwordHasher.Verify(
            request.CurrentPassword,
            user.PasswordHash);

    if (!currentPasswordValid)
    {
        throw new UnauthorizedAccessException(
            "Mevcut şifre hatalı.");
    }

    if (request.CurrentPassword == request.NewPassword)
    {
        throw new InvalidOperationException(
            "Yeni şifre mevcut şifre ile aynı olamaz.");
    }

    user.PasswordHash =
        _passwordHasher.Hash(request.NewPassword);

    user.UpdatedAt = DateTime.UtcNow;

    await _userRepository.UpdateAsync(user);
}
}