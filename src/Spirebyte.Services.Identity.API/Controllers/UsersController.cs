using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Identity.API.Controllers.Base;
using Spirebyte.Services.Identity.Application.Users.Commands;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Identity.API.Controllers;

public class UsersController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public UsersController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet("{userId:guid}")]
    [SwaggerOperation("Get user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UserDto>> GetAsync(Guid userId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new GetUser(userId)));
    }

    [HttpGet("withids")]
    [SwaggerOperation("With ids")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetByIdsAsync([FromQuery] GetUsersByIds query)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(query));
    }

    [HttpPost("reset-password")]
    [SwaggerOperation("Reset Password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SignUpAsync(ResetPassword command)
    {
        await _dispatcher.SendAsync(command);
        return NoContent();
    }

    [HttpPost("forgot-password")]
    [SwaggerOperation("Forgot password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SignUpAsync(ForgotPassword command)
    {
        await _dispatcher.SendAsync(command);
        return NoContent();
    }
}