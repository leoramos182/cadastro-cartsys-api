using CadastroCartsys.Domain.Projections;
using CadastroCartsys.Domain.Users;
using CadastroCartsys.Domain.Users.Commands;
using CadastroCartsys.Domain.Users.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadastroCartsys.Api.Controllers;


[Route("api/users")]
[ApiController]
[Authorize(Policy = "Authenticated")]
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var user = await _userRepository.GetAllUsers();
        return await Task.FromResult(OkResponse(user.ToVm()));
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchUsersFilters filters)
    {
        var users = await _userRepository.Filter(filters);
        return await Task.FromResult(OkResponse(users.ToVm()));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
    {
        return CreatedResponse(await _mediator.Send(command));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put([FromRoute]Guid id, [FromBody] UpdateUserCommand command)
    {
        command.Id = id;
        return CreatedResponse(await _mediator.Send(command));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var request = new DeleteUserCommand() { Id = id };
        return OkResponse(await _mediator.Send(request));
    }

    [HttpPatch("{id:guid}/change-status")]
    public async Task<IActionResult> ChangeStatus(Guid id)
    {
        var user = await _userRepository.GetUser(id);
        if (user == null) return NotFoundResponse();
        user.ActivateDeactivate();
        user = _userRepository.Modify(user);

        return OkResponse(user.ToVm());

    }
}