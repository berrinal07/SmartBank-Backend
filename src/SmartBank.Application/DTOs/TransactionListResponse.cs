
namespace SmartBank.Application.DTOs;

public class TransactionListResponse
{
    public List<TransactionResponse> Items { get; set; } = [];

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}

