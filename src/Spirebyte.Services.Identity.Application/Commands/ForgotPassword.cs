using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Commands;

[Contract]
public class ForgotPassword : ICommand
{
    public ForgotPassword(string email)
    {
        Email = email;
    }

    public string Email { get; set; }
}