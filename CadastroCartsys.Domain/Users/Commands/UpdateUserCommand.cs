using System.Text.Json.Serialization;

namespace CadastroCartsys.Domain.Users.Commands;

public class UpdateUserCommand: CreateUserCommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
}