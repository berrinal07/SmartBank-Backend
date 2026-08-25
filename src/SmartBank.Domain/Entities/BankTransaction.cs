using SmartBank.Domain.Enums;

namespace SmartBank.Domain.Entities;

public class BankTransaction
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public string? Description { get; set; }

    public Guid? RelatedAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Account Account { get; set; } = null!;
}