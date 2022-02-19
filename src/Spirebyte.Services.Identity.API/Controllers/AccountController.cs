using System.Threading.Tasks;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Identity.API.Controllers.Base;
using Spirebyte.Services.Identity.Application.Users.Commands;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Spirebyte.Shared.Contexts.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Identity.API.Controllers;

public class AccountController : BaseController
{
    private readonly IAppContext _appContext;
    private readonly IDispatcher _dispatcher;
    private readonly IRequestDispatcher _requestDispatcher;

    public AccountController(IDispatcher dispatcher, IRequestDispatcher requestDispatcher, IAppContext appContext)
    {
        _dispatcher = dispatcher;
        _requestDispatcher = requestDispatcher;
        _appContext = appContext;
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation("Get account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> GetAsync()
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new GetUser(_appContext.Identity.Id)));
    }

    [HttpPut]
    [Authorize]
    [SwaggerOperation("Update account")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UpdateAccountAsync(UpdateUser command)
    {
        await _dispatcher.SendAsync(command.Bind(x => x.UserId, _appContext.Identity.Id));
        return NoContent();
    }

    [HttpPost("sign-up")]
    [SwaggerOperation("Sign up")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SignUpAsync(SignUp request)
    {
        await _dispatcher.SendAsync(request);
        return NoContent();
    }
}