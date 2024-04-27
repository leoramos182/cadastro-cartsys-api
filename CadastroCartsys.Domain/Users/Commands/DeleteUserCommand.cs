using CadastroCartsys.Domain.Users.ViewModels;
using MediatR;

namespace CadastroCartsys.Domain.Users.Commands;

public class DeleteUserCommand: IRequest<UserVm>
{
    public Guid Id { get; set; }
}