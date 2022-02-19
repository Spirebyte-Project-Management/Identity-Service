using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Identity.API.Controllers.Base;
using Spirebyte.Services.Identity.Application.Authentication.DTO;
using Spirebyte.Services.Identity.Application.Authentication.Requests;
using Spirebyte.Shared.Contexts.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Identity.API.Controllers;

public class AuthController : BaseController
{
    private readonly IAppContext _appContext;
    private readonly IDispatcher _dispatcher;
    private readonly IRequestDispatcher _requestDispatcher;

    public AuthController(IDispatcher dispatcher, IRequestDispatcher requestDispatcher, IAppContext appContext)
    {
        _dispatcher = dispatcher;
        _requestDispatcher = requestDispatcher;
        _appContext = appContext;
    }

    [HttpPost("sign-in")]
    [SwaggerOperation("Sign in")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthDto>> SignInAsync(SignIn request)
    {
        var jwt = await _requestDispatcher.DispatchAsync<SignIn, AuthDto>(request);
        return Ok(jwt);
    }


    [HttpPost("refresh-tokens/use")]
    [SwaggerOperation("Use refresh token")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthDto>> RefreshTokenAsync(UseRefreshToken request)
    {
        var jwt = await _requestDispatcher.DispatchAsync<UseRefreshToken, AuthDto>(request);
        return Ok(jwt);
    }
}