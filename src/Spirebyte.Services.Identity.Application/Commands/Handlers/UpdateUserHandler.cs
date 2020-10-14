using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Partytitan.Convey.WindowsAzure.Blob.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Commands.Handlers
{
    internal sealed class UpdateUserHandler : ICommandHandler<UpdateUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserHandler> _logger;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAppContext _appContext;

        public UpdateUserHandler(IUserRepository userRepository, ILogger<UpdateUserHandler> logger,
            IBlobStorageService blobStorageService, IAppContext appContext)
        {
            _userRepository = userRepository;
            _logger = logger;
            _blobStorageService = blobStorageService;
            _appContext = appContext;
        }
        public async Task HandleAsync(UpdateUser command)
        {

            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            string picUrl = command.Pic;

            if (!string.IsNullOrWhiteSpace(command.File))
            {
                var mimeType = Extensions.GetMimeTypeFromBase64(command.File);
                var data = Extensions.GetDataFromBase64(command.File);
                var fileName = _appContext.Identity.Id + "_" + DateTime.Now.ConvertToUnixTimestamp();

                var bytes = Convert.FromBase64String(data);
                Stream contents = new MemoryStream(bytes);
                var uri = await _blobStorageService.UploadFileBlobAsync(contents, mimeType, fileName);
                picUrl = uri.OriginalString;
            }

            user = new User(user.Id, user.Email, command.Fullname, picUrl, user.Password, user.Role, user.SecurityStamp, user.CreatedAt, user.Permissions);
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Updated account for the user with id: {user.Id}.");
        }
    }
}
