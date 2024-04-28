using System.ComponentModel.DataAnnotations;
using CadastroCartsys.Domain.Users.ViewModels;
using MediatR;

namespace CadastroCartsys.Domain.Users.Commands;

public class AuthenticateUser: IRequest<UserVm>
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}