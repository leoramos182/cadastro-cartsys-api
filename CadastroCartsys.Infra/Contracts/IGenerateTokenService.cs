using CadastroCartsys.Domain.Models;

namespace CadastroCartsys.Api.Infra.Contracts;

public interface IGenerateTokenService
{
    public Task<TokenResult> Generate();
}