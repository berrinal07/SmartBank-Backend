using SmartBank.Application.Transfers.Interfaces;

namespace SmartBank.Application.Transfers.Services;

public class TransferService : ITransferService
{
    private readonly ITransferRepository _transferRepository;

    public TransferService(
        ITransferRepository transferRepository)
    {
        _transferRepository = transferRepository;
    }

    public async Task TransferAsync(
        Guid userId,
        TransferRequest request)
    {
        await _transferRepository.ExecuteTransferAsync(
            userId,
            request);
    }
}