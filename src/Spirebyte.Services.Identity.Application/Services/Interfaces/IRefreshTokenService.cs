using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Spirebyte.Services.Identity.Application.DTO;

namespace Spirebyte.Services.Identity.Application.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(Guid userId);
        Task RevokeAsync(string refreshToken);
        Task<AuthDto> UseAsync(string refreshToken);
    }
}
