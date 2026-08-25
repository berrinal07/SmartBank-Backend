using SmartBank.Domain.Enums;

namespace SmartBank.Application.DTOs;

public class AccountSummaryResponse
{
    public Guid Id { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public AccountType AccountType { get; set; }

    public decimal Balance { get; set; }
}