using System.Net;
using CadastroCartsys.Crosscutting.Exceptions;
using CadastroCartsys.Crosscutting.Messages;
using CadastroCartsys.Crosscutting.Notifications;
using CadastroCartsys.Crosscutting.Results;
using CadastroCartsys.Crosscutting.Utils;
using CadastroCartsys.Domain.Contracts;
using CadastroCartsys.Domain.Users;
using CadastroCartsys.Domain.Users.Commands;
using CadastroCartsys.Domain.Users.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CadastroCartsys.Api.Controllers;

[Route("api/auth")]
public class AuthController: BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IUsersRepository _usersRepository;

    public AuthController(IMediator mediator, IUsersRepository usersRepository)
    {
        _mediator = mediator;
        _usersRepository = usersRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post( [FromBody] AuthenticateUser command,
        [FromServices] IJwTokenService tokenService)
    {
        if (command == null) return UnprocessableEntity(EnvelopResult.Fail(new[] {new Notification("Invalid Entity")}));

        var user = await _usersRepository.FindByEmailAsync(command.Email);

        if (user == null)
            throw new DomainException(UserMessages.InvalidCredentials);

        if (!user.Active)
            throw new DomainException(UserMessages.InvalidCredentials);

        if (!PasswordUtils.Check(user.Password, command.Password))
            throw new DomainException(UserMessages.InvalidCredentials);

        var result = new AuthenticateUserResult();

        result.User = new SessionUser(
            id: user.Id,
            email: user.Email,
            name: user.Name
        );

        tokenService.Generate(result.User.WriteClaimsIdentity(), ref result);

        return OkResponse(result);
    }
}