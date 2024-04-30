using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CadastroCartsys.Api.Infra.Contracts;
using CadastroCartsys.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CadastroCartsys.Api.Infra.Services;

public class GenerateTokenService : IGenerateTokenService
{
    private readonly IConfiguration _configuration;

    public GenerateTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<TokenResult> Generate()
    {
        var result = new TokenResult();

        var key = _configuration["Jwt:Key"];
        var _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var _issuer = _configuration["Jwt:Issuer"];
        var _audience = _configuration["Jwt:Audience"];
        var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: _issuer,
            claims: Array.Empty<Claim>(),
            audience: _audience,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: signinCredentials);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        result.Token = tokenString;
        return result;
    }


}