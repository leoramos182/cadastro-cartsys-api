using CadastroCartsys.Domain.Projections;
using CadastroCartsys.Domain.Users;
using CadastroCartsys.Domain.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CadastroCartsys.Api.Controllers;


[Route("api/users")]
[ApiController]
public class UsersController: BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IUsersRepository _userRepository;

    public UsersController(IMediator mediator, IUsersRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var user = await _userRepository.GetUser(id);

        return await Task.FromResult(user == null
            ? NotFoundResponse()
            : OkResponse(user.ToVm()));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
    {

        return command == null
            ? UnprocessableEntity()
            : CreatedResponse(await _mediator.Send(command));
    }
}