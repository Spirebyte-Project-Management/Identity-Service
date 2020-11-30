using Spirebyte.Services.Identity.Application.DTO;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Identity.Application.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(Guid userId);
        Task RevokeAsync(string refreshToken);
        Task<AuthDto> UseAsync(string refreshToken);
    }
}
