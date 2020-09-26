using Convey.WebApi.Requests;

namespace Spirebyte.Services.Identity.Application.Requests
{
    [Contract]
    public class SignIn : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public SignIn(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
