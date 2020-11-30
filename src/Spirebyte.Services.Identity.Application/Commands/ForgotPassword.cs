using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Commands
{
    [Contract]
    public class ForgotPassword : ICommand
    {
        public string Email { get; set; }

        public ForgotPassword(string email)
        {
            Email = email;
        }
    }
}
