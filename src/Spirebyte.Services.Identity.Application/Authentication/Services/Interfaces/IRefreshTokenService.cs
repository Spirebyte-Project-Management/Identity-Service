using System;
using System.Threading.Tasks;
using Spirebyte.Services.Identity.Application.Authentication.DTO;

namespace Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;

public interface IRefreshTokenService
{
    Task<string> CreateAsync(Guid userId);
    Task RevokeAsync(string refreshToken);
    Task<AuthDto> UseAsync(string refreshToken);
}