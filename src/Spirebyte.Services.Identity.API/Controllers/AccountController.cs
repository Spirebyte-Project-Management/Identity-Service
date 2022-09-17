using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Identity.API.Controllers.Base;

namespace Spirebyte.Services.Identity.API.Controllers;

public class AccountController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public AccountController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
}