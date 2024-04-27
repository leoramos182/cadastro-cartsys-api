using CadastroCartsys.Crosscutting.Messages;
using CadastroCartsys.Crosscutting.Notifications;
using CadastroCartsys.Crosscutting.Results;
using Microsoft.AspNetCore.Mvc;

namespace CadastroCartsys.Api.Controllers;

public class BaseApiController: Controller
{
    [NonAction]
    protected IActionResult OkResponse<T>(T data = default(T))
    {
        return Ok(EnvelopDataResult<T>.Ok(data));
    }

    [NonAction]
    protected IActionResult NotFoundResponse()
    {
        return new NotFoundObjectResult(EnvelopResult.Fail(new[] {new Notification(UserMessages.NotFound)}));
    }

    [NonAction]
    protected IActionResult CreatedResponse<T>(T data = default(T), string url = "")
    {
        return Created(url, EnvelopDataResult<T>.Ok(data));
    }
}