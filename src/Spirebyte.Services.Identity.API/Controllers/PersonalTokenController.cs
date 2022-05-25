using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Identity.API.Controllers.Base;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Identity.API.Controllers;

public class PersonalTokenController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public PersonalTokenController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
}