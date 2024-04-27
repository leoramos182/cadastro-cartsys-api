using System.ComponentModel.DataAnnotations;
using CadastroCartsys.Domain.Users.ViewModels;
using MediatR;

namespace CadastroCartsys.Domain.Users.Commands;

public class CreateUserCommand: IRequest<UserVm>
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
}