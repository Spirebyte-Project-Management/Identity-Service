using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;
using Partytitan.Convey.Minio.Services.Interfaces;

namespace Spirebyte.Services.Identity.Application.Commands.Handlers
{
    internal sealed class UpdateUserHandler : ICommandHandler<UpdateUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserHandler> _logger;
        private readonly IMinioService _minioService;
        private readonly IAppContext _appContext;

        public UpdateUserHandler(IUserRepository userRepository, ILogger<UpdateUserHandler> logger,
            IMinioService minioService, IAppContext appContext)
        {
            _userRepository = userRepository;
            _logger = logger;
            _minioService = minioService;
            _appContext = appContext;
        }
        public async Task HandleAsync(UpdateUser command)
        {

            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            string picUrl = user.Pic;

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

            string securityStamp = Guid.Empty.ToString();

            user = new User(user.Id, user.Email, command.Fullname, picUrl, user.Password, user.Role, securityStamp, user.AccessFailedCount, user.LockoutEnd, user.CreatedAt, user.Permissions);
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Updated account for the user with id: {user.Id}.");
        }
    }
}
