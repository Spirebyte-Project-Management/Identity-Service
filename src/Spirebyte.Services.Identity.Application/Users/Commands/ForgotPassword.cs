using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Users.Commands;

[Contract]
public record ForgotPassword(string Email) : ICommand;