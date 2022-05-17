using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.Events;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Users.Commands.Handlers;

internal sealed class ForgotPasswordHandler : ICommandHandler<ForgotPassword>
{
    public async Task HandleAsync(ForgotPassword command, CancellationToken cancellationToken = default)
    {
       
    }
}