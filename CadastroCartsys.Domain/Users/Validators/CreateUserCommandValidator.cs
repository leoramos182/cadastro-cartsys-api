using CadastroCartsys.Crosscutting.Messages;
using CadastroCartsys.Domain.Users.Commands;
using FluentValidation;

namespace CadastroCartsys.Domain.Users.Validators;

public class CreateUserCommandValidator: AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserMessages.Email.EmptyEMail);

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(UserMessages.Email.InvalidEMail)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(UserMessages.Name.EmptyName);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(UserMessages.Password.EmptyPassword);
    }
}