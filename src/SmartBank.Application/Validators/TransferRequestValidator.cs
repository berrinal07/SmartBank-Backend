using FluentValidation;
using SmartBank.Application.Transfers;

namespace SmartBank.Application.Validators;

public class TransferRequestValidator : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.FromAccountNumber)
            .NotEmpty()
            .WithMessage("Gönderen hesap numarası zorunludur.");

        RuleFor(x => x.ToAccountNumber)
            .NotEmpty()
            .WithMessage("Alıcı hesap numarası zorunludur.");

        RuleFor(x => x.FromAccountNumber)
            .NotEqual(x => x.ToAccountNumber)
            .WithMessage("Gönderen ve alıcı hesap aynı olamaz.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Transfer tutarı 0'dan büyük olmalıdır.");

        RuleFor(x => x.Description)
            .MaximumLength(250)
            .WithMessage("Açıklama en fazla 250 karakter olabilir.");
    }
}