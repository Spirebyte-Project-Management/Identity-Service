using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Spirebyte.Services.Identity.Application.Commands;
using Spirebyte.Services.Identity.Application.Commands.Handlers;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Unit.Application.Handlers
{
    public class SignUpHandlerTests
    {
        public SignUpHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _passwordService = Substitute.For<IPasswordService>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _logger = Substitute.For<ILogger<SignUpHandler>>();

            _passwordService.Hash(default).ReturnsForAnyArgs("secret");

            _handler = new SignUpHandler(_userRepository, _passwordService, _messageBroker, _logger);
        }

        private readonly SignUpHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<SignUpHandler> _logger;

        [Fact]
        public async Task given_invalid_email_signup_should_throw_an_exception()
        {
            var id = new AggregateId();
            var email = "email@com";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;

            var command = new SignUp(id, email, fullname, "test.nl/image", password, role, new string[] { });
            Func<Task> func = async () => await _handler.HandleAsync(command);
            func.Should().Throw<InvalidEmailException>();
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
            Func<Task> func = async () => await _handler.HandleAsync(command);
            func.Should().NotThrow();
        }
    }
}