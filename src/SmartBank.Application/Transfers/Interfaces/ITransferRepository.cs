using SmartBank.Application.Transfers;

namespace SmartBank.Application.Transfers.Interfaces;

public interface ITransferRepository
{
    Task ExecuteTransferAsync(
        Guid userId,
        TransferRequest request);
}