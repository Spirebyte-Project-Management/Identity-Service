using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Authentication.Exceptions;
using Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Users.Commands.Handlers;

internal sealed class ResetPasswordHandler : ICommandHandler<ResetPassword>
{
    public async Task HandleAsync(ResetPassword command, CancellationToken cancellationToken = default)
    {
        
    }
}