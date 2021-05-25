using Convey.WebApi.Requests;

namespace Spirebyte.Services.Identity.Application.Requests
{
    [Contract]
    public class UseRefreshToken : IRequest
    {
        public string RefreshToken { get; }

        public UseRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
