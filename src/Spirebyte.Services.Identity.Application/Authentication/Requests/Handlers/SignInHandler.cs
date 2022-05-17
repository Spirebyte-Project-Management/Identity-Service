using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Convey.WebApi.Requests;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Authentication.DTO;
using Spirebyte.Services.Identity.Application.Authentication.Events;
using Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Authentication.Requests.Handlers;

public class SignInHandler : IRequestHandler<SignIn, AuthDto>
{

    public async Task<AuthDto> HandleAsync(SignIn request, CancellationToken cancellationToken = default)
    {
        return null;
    }
}