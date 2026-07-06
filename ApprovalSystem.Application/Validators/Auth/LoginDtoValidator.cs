using ApprovalSystem.Application.DTOs.Auth;
using FluentValidation;

namespace ApprovalSystem.Application.Validators.Auth;

public class LoginDtoValidator
    : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}