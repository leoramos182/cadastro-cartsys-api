using CadastroCartsys.Crosscutting.Exceptions;
using CadastroCartsys.Crosscutting.Messages;
using CadastroCartsys.Crosscutting.Utils;
using CadastroCartsys.Domain.Entities;
using CadastroCartsys.Domain.Projections;
using CadastroCartsys.Domain.Users.ViewModels;
using MediatR;

namespace CadastroCartsys.Domain.Users.Commands.Handlers;

public class UserCommandHandler: IRequestHandler<CreateUserCommand, UserVm>,
    IRequestHandler<UpdateUserCommand, UserVm>,
    IRequestHandler<DeleteUserCommand, UserVm>
{
    private readonly IUsersRepository _usersRepository;

    public UserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<UserVm> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.FindByEmailAsync(request.Email);

        if (user != null) throw new Exception(UserMessages.Email.EmailInUse);

        var hashedPassword = PasswordUtils.Hash(request.Password);

        user = new User(request.Name, request.Email, hashedPassword);

        user = await _usersRepository.AddAsync(user);

        return user.ToVm();

    }


    public async Task<UserVm> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUser(request.Id);

        if (user == null)
            throw new DomainException(UserMessages.NotFound);

        user.Update(request.Name.Trim(), request.Email);

        user = _usersRepository.Modify(user);

        return user.ToVm();
    }

    public async Task<UserVm> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUser(request.Id);

        if (user == null)
            throw new DomainException(UserMessages.NotFound);

        user = _usersRepository.Remove(user);

        return await Task.FromResult(user.ToVm());
    }
}