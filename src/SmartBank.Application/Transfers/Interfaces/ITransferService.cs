using SmartBank.Application.Transfers;

namespace SmartBank.Application.Transfers.Interfaces;

public interface ITransferService
{
    Task TransferAsync(
        Guid userId,
        TransferRequest request);
}