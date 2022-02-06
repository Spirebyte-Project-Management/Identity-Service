using Convey.WebApi.Requests;

namespace Spirebyte.Services.Identity.Application.Authentication.Requests;

[Contract]
public record SignIn(string Email, string Password) : IRequest;