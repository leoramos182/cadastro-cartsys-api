using System.Security.Claims;
using CadastroCartsys.Crosscutting.Models;
using Microsoft.IdentityModel.Tokens;

namespace CadastroCartsys.Domain.Contracts;

public interface IJwTokenService
{
    void Generate<T>(ClaimsIdentity identity, ref T jwToken)
        where T : JwToken;
    TokenValidationResult Validate(string token);
}