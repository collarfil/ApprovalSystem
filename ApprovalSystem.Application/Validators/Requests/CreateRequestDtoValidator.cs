using ApprovalSystem.Application.DTOs.Requests;
using FluentValidation;

namespace ApprovalSystem.Application.Validators.Requests;

public class CreateRequestDtoValidator
    : AbstractValidator<CreateRequestDto>
{
    public CreateRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.WorkflowDefinitionId)
            .NotEmpty();
    }
}