using FluentValidation;
using SmartBank.Application.DTOs;
using SmartBank.Domain.Enums;

namespace SmartBank.Application.Validators;

public class CreateAccountRequestValidator
    : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.AccountType)
            .Must(type => Enum.IsDefined(type))
            .WithMessage("Geçersiz hesap tipi.");

        RuleFor(x => x.InitialBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "Başlangıç bakiyesi negatif olamaz.");
    }
}