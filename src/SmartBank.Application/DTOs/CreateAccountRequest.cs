using SmartBank.Domain.Enums;

namespace SmartBank.Application.DTOs;

public class CreateAccountRequest
{
    public AccountType AccountType { get; set; }

    public decimal InitialBalance { get; set; }
}