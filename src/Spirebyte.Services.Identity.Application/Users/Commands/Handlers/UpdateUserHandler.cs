using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Partytitan.Convey.Minio.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Exceptions;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Identity.Application.Users.Commands.Handlers;

internal sealed class UpdateUserHandler : ICommandHandler<UpdateUser>
{
    private readonly IAppContext _appContext;
    private readonly ILogger<UpdateUserHandler> _logger;
    private readonly IMinioService _minioService;

    public UpdateUserHandler(ILogger<UpdateUserHandler> logger,
        IMinioService minioService, IAppContext appContext)
    {
        _logger = logger;
        _minioService = minioService;
        _appContext = appContext;
    }

    public async Task HandleAsync(UpdateUser command, CancellationToken cancellationToken = default)
    {
        var user = new UserDto();
        if (user is null) throw new UserNotFoundException(command.UserId);

        var picUrl = user.Picture;

        if (!string.IsNullOrWhiteSpace(command.File))
        {
            if (command.File == "delete")
            {
                picUrl = string.Empty;
            }
            else
            {
                var mimeType = Extensions.GetMimeTypeFromBase64(command.File);
                var data = Extensions.GetDataFromBase64(command.File);
                var fileName = _appContext.Identity.Id + "_" + DateTime.Now.ConvertToUnixTimestamp();

                var bytes = Convert.FromBase64String(data);
                Stream contents = new MemoryStream(bytes);
                picUrl = await _minioService.UploadFileAsync(contents, mimeType, fileName);
            }
        }
        

        _logger.LogInformation($"Updated account for the user with id: {user.Id}.");
    }
}