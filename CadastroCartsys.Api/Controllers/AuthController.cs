using CadastroCartsys.Api.Infra.Contracts;
using CadastroCartsys.Crosscutting.Messages;
using CadastroCartsys.Crosscutting.Models;
using CadastroCartsys.Crosscutting.Utils;
using CadastroCartsys.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CadastroCartsys.Api.Controllers;

[Route("api/auth")]
public class AuthController: BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IUsersRepository _usersRepository;
    private readonly IGenerateTokenService _generateTokenService;

    public AuthController(IUsersRepository usersRepository, IGenerateTokenService generateTokenService)
    {
        _usersRepository = usersRepository;
        _generateTokenService = generateTokenService;
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

        return Ok(_generateTokenService.Generate());
    }
}