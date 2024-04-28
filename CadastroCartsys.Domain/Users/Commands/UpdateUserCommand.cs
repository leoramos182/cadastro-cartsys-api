using System.Text.Json.Serialization;
using CadastroCartsys.Domain.Users.ViewModels;
using MediatR;

namespace CadastroCartsys.Domain.Users.Commands;

public class UpdateUserCommand : IRequest<UserVm>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }

}