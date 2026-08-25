using SmartBank.Domain.Enums;

namespace SmartBank.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public AccountType AccountType { get; set; }

    public decimal Balance { get; set; }

    public decimal InterestRate { get; set; }

    public DateTime? MaturityDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;

    public ICollection<BankTransaction> Transactions { get; set; }
        = new List<BankTransaction>();
}