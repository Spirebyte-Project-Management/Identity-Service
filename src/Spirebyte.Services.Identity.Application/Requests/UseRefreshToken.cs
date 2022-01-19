using Convey.WebApi.Requests;

namespace Spirebyte.Services.Identity.Application.Requests;

[Contract]
public class UseRefreshToken : IRequest
{
    public UseRefreshToken(string refreshToken)
    {
        RefreshToken = refreshToken;
    }

    public string RefreshToken { get; }
}