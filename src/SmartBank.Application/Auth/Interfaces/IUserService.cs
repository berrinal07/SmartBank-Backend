using SmartBank.Application.DTOs;

namespace SmartBank.Application.Auth.Interfaces;

public interface IUserService
{
    Task<UserProfileResponse> GetProfileAsync(Guid userId);

    Task<UserProfileResponse> UpdateProfileAsync(
        Guid userId,
        UpdateUserProfileRequest request);

    Task UpdatePasswordAsync(
        Guid userId,
        UpdatePasswordRequest request);
}