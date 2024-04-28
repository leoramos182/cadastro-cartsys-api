using CadastroCartsys.Crosscutting.Config;
using CadastroCartsys.Crosscutting.Models;
using CadastroCartsys.Crosscutting.Results;

namespace CadastroCartsys.Domain.Users.Results;

public class AuthenticateUserResult: JwToken
{
    public SessionUser User { get; set; }
}