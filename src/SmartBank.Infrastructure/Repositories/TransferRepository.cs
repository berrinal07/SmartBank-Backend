using Microsoft.EntityFrameworkCore;
using SmartBank.Application.Transfers;
using SmartBank.Application.Transfers.Interfaces;
using SmartBank.Domain.Entities;
using SmartBank.Application.Notifications.Interfaces;
using SmartBank.Domain.Enums;
using SmartBank.Infrastructure.Data;

namespace SmartBank.Infrastructure.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly AppDbContext _context;
private readonly INotificationRepository _notificationRepository;

public TransferRepository(
    AppDbContext context,
    INotificationRepository notificationRepository)
{
    _context = context;
    _notificationRepository = notificationRepository;
}

    public async Task ExecuteTransferAsync(
        Guid userId,
        TransferRequest request)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var fromAccount = await _context.Accounts
                .FirstOrDefaultAsync(a =>
                    a.AccountNumber == request.FromAccountNumber &&
                    a.UserId == userId);

            if (fromAccount is null)
            {
                throw new KeyNotFoundException(
                    "Gönderen hesap bulunamadı.");
            }

            var toAccount = await _context.Accounts
                .FirstOrDefaultAsync(a =>
                    a.AccountNumber == request.ToAccountNumber);

            if (toAccount is null)
            {
                throw new KeyNotFoundException(
                    "Alıcı hesap bulunamadı.");
            }

            if (fromAccount.Id == toAccount.Id)
            {
                throw new ArgumentException(
                    "Gönderen ve alıcı hesap aynı olamaz.");
            }

            if (fromAccount.Balance < request.Amount)
            {
                throw new InvalidOperationException(
                    "Yetersiz bakiye.");
            }

            fromAccount.Balance -= request.Amount;
            toAccount.Balance += request.Amount;

            var outgoingTransaction = new BankTransaction
            {
                Id = Guid.NewGuid(),
                AccountId = fromAccount.Id,
                Amount = request.Amount,
                TransactionType = TransactionType.Transfer,
                Description = request.Description,
                RelatedAccountId = toAccount.Id,
                CreatedAt = DateTime.UtcNow
            };

            var incomingTransaction = new BankTransaction
            {
                Id = Guid.NewGuid(),
                AccountId = toAccount.Id,
                Amount = request.Amount,
                TransactionType = TransactionType.Transfer,
                Description = request.Description,
                RelatedAccountId = fromAccount.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Transactions.AddRangeAsync(
                outgoingTransaction,
                incomingTransaction);

                if (fromAccount.UserId == toAccount.UserId)
{
    var notification = new Notification
    {
        Id = Guid.NewGuid(),
        UserId = fromAccount.UserId,
        Title = "Transfer Başarılı",
        Message = $"{request.Amount} ₺ transfer başarıyla gönderildi.",
        IsRead = false,
        CreatedAt = DateTime.UtcNow
    };

    await _context.Notifications.AddAsync(notification);
}
else
{
    var senderNotification = new Notification
    {
        Id = Guid.NewGuid(),
        UserId = fromAccount.UserId,
        Title = "Transfer Başarılı",
        Message = $"{request.Amount} ₺ transfer başarıyla gönderildi.",
        IsRead = false,
        CreatedAt = DateTime.UtcNow
    };

    var receiverNotification = new Notification
    {
        Id = Guid.NewGuid(),
        UserId = toAccount.UserId,
        Title = "Para Transferi Aldınız",
        Message = $"{request.Amount} ₺ transfer hesabınıza geldi.",
        IsRead = false,
        CreatedAt = DateTime.UtcNow
    };

    await _context.Notifications.AddRangeAsync(
        senderNotification,
        receiverNotification);
}

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}