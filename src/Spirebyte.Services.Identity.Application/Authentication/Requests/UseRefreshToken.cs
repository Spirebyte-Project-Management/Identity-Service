using Convey.WebApi.Requests;

namespace Spirebyte.Services.Identity.Application.Authentication.Requests;

[Contract]
public record UseRefreshToken(string RefreshToken) : IRequest;