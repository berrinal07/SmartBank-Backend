using SmartBank.Domain.Enums;

namespace SmartBank.Application.DTOs;

public class AccountResponse
{
    public Guid Id { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public AccountType AccountType { get; set; }

    public decimal Balance { get; set; }

    public decimal InterestRate { get; set; }

    public DateTime? MaturityDate { get; set; }

    public DateTime CreatedAt { get; set; }
}