using SmartBank.Application.DTOs;

namespace SmartBank.Application.Accounts.Interfaces;

public interface IAccountService
{
    Task<AccountResponse> CreateAsync(
        Guid userId,
        CreateAccountRequest request);

    Task<List<AccountSummaryResponse>> GetMyAccountsAsync(
        Guid userId);

    Task<AccountResponse> GetByIdAsync(
        Guid userId,
        Guid accountId);
}