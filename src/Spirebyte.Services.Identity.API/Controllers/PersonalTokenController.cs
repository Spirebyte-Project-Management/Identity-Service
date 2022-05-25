using Convey.WebApi.CQRS;
using Spirebyte.Services.Identity.API.Controllers.Base;

namespace Spirebyte.Services.Identity.API.Controllers;

public class PersonalTokenController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public PersonalTokenController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
}