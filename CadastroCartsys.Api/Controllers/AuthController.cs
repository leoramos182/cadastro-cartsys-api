using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CadastroCartsys.Crosscutting.Exceptions;
using CadastroCartsys.Crosscutting.Messages;
using CadastroCartsys.Crosscutting.Models;
using CadastroCartsys.Crosscutting.Utils;
using CadastroCartsys.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CadastroCartsys.Api.Controllers;

[Route("api/auth")]
public class AuthController: BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IUsersRepository _usersRepository;
    private readonly IConfiguration _configuration;

    public AuthController(IMediator mediator,
        IUsersRepository usersRepository,
        IConfiguration configuration)
    {
        _mediator = mediator;
        _usersRepository = usersRepository;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Post( [FromBody] LoginViewModel  command)
    {
        var user = await _usersRepository.FindByEmailAsync(command.Email);

        if (user is null)
            return BadRequest(UserMessages.Email.InvalidEMail);

        if (!user.Active)
            return BadRequest(UserMessages.Active.DeactivatedUser);

        if (!PasswordUtils.Check(user.Password, command.Password))
            return BadRequest(UserMessages.Password.WrongPassword);

        var key = Base64UrlEncoder.DecodeBytes(_configuration["Jwt:Key"]);
        var _secretKey = new SymmetricSecurityKey(key);
        var _issuer = _configuration["Jwt:Issuer"];
        var _audience = _configuration["Jwt:Audience"];
        var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer : _issuer,
            audience: _audience,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: signinCredentials);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return Ok(new { Token = tokenString });
    }
}