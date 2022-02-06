using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.Commands;
using Spirebyte.Services.Identity.Application.Users.Commands.Handlers;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Unit.Application.Handlers;

public class SignUpHandlerTests
{
    private readonly SignUpHandler _handler;
    private readonly ILogger<SignUpHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly IPasswordService _passwordService;
    private readonly IUserRepository _userRepository;

    public SignUpHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordService = Substitute.For<IPasswordService>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _logger = Substitute.For<ILogger<SignUpHandler>>();

        _passwordService.Hash(default).ReturnsForAnyArgs("secret");

        _handler = new SignUpHandler(_userRepository, _passwordService, _messageBroker, _logger);
    }

    [Fact]
    public async Task given_invalid_email_signup_should_throw_an_exception()
    {
        var id = new AggregateId();
        var email = "email@com";
        var fullname = "fullname";
        var password = "secret";
        var role = Role.User;

        var command = new SignUp(id, email, fullname, "test.nl/image", password, role, new string[] { });
        var func = async () => await _handler.HandleAsync(command);

        await func.Should().ThrowAsync<InvalidEmailException>();
    }

    [Fact]
    public async Task given_valid_user_signup_should_succeed()
    {
        var id = new AggregateId();
        var email = "email@test.com";
        var fullname = "fullname";
        var password = "secret";
        var role = Role.User;

        var command = new SignUp(id, email, fullname, "test.nl/image", password, role, new string[] { });
        var func = async () => await _handler.HandleAsync(command);

        await func.Should().NotThrowAsync();
    }
}