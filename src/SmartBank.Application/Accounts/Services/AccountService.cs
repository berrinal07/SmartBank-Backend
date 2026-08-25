using SmartBank.Application.Accounts.Interfaces;
using SmartBank.Application.DTOs;
using SmartBank.Domain.Entities;
using SmartBank.Domain.Enums;
using System.Security.Cryptography;

namespace SmartBank.Application.Accounts.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(
        IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

   public async Task<AccountResponse> CreateAsync(
    Guid userId,
    CreateAccountRequest request)
{
    if (request.InitialBalance < 0)
    {
        throw new ArgumentException(
            "Başlangıç bakiyesi negatif olamaz.");
    }

    if (!Enum.IsDefined(request.AccountType))
    {
        throw new ArgumentException(
            "Geçersiz hesap tipi.");
    }

    string accountNumber;

    do
    {
        accountNumber = GenerateAccountNumber();
    }
    while (await _accountRepository
        .ExistsByAccountNumberAsync(accountNumber));

    var account = new Account
    {
        Id = Guid.NewGuid(),

        UserId = userId,

        AccountNumber = accountNumber,

        AccountType = request.AccountType,

        Balance = request.InitialBalance,

        CreatedAt = DateTime.UtcNow
    };

    if (request.AccountType == AccountType.Checking)
    {
        account.InterestRate = 0;
        account.MaturityDate = null;
    }
    else
    {
        account.InterestRate = 20;
        account.MaturityDate =
            DateTime.UtcNow.AddMonths(3);
    }

    await _accountRepository.AddAsync(account);

    await _accountRepository.SaveChangesAsync();

    return new AccountResponse
    {
        Id = account.Id,
        AccountNumber = account.AccountNumber,
        AccountType = account.AccountType,
        Balance = account.Balance,
        InterestRate = account.InterestRate,
        MaturityDate = account.MaturityDate,
        CreatedAt = account.CreatedAt
    };
}

private static string GenerateAccountNumber()
{
    var number = RandomNumberGenerator
        .GetInt32(100000000, 1000000000);

    return number.ToString();
}
    public async Task<List<AccountSummaryResponse>> GetMyAccountsAsync(
    Guid userId)
{
    var accounts =
        await _accountRepository.GetByUserIdAsync(userId);

    return accounts.Select(account =>
        new AccountSummaryResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountType = account.AccountType,
            Balance = account.Balance
        })
        .ToList();
}

    public async Task<AccountResponse> GetByIdAsync(
    Guid userId,
    Guid accountId)
{
    var account =
        await _accountRepository.GetByIdAsync(
            accountId,
            userId);

    if (account is null)
    {
        throw new KeyNotFoundException(
            "Hesap bulunamadı.");
    }

    return new AccountResponse
    {
        Id = account.Id,
        AccountNumber = account.AccountNumber,
        AccountType = account.AccountType,
        Balance = account.Balance,
        InterestRate = account.InterestRate,
        MaturityDate = account.MaturityDate,
        CreatedAt = account.CreatedAt
    };
}

}