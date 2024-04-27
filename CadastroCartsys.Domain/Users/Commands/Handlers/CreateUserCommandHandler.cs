using CadastroCartsys.Crosscutting.Exceptions;
using CadastroCartsys.Crosscutting.Messages;
using CadastroCartsys.Crosscutting.Utils;
using CadastroCartsys.Domain.Entities;
using CadastroCartsys.Domain.Projections;
using CadastroCartsys.Domain.Users.ViewModels;
using MediatR;

namespace CadastroCartsys.Domain.Users.Commands.Handlers;

public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, UserVm>
{
    private readonly IUsersRepository _usersRepository;

    public CreateUserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<UserVm> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.FindByEmailAsync(request.Email);

        if (user != null)
            throw new DomainException(UserMessages.EmailInUse);

        var hashedPassword = PasswordUtils.Hash(request.Password);

        user = new User(request.Name, request.Email, hashedPassword);

        user = await _usersRepository.AddAsync(user);

        return user.ToVm();

    }
}