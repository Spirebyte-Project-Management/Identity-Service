using Convey.WebApi.Requests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Application.Requests;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Spirebyte.Services.Identity.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Integration.Requests
{
    [Collection("Spirebyte collection")]
    public class SignInTests : IDisposable
    {
        public SignInTests(SpirebyteApplicationIntegrationFactory<Program> factory)
        {
            var rabbitmqOptions = factory.Services.GetRequiredService<RabbitMqOptions>();
            _rabbitMqFixture = new RabbitMqFixture(rabbitmqOptions);
            var mongoOptions = factory.Services.GetRequiredService<MongoDbOptions>();
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users", mongoOptions);
            factory.Server.AllowSynchronousIO = true;
            _requestHandler = factory.Services.GetRequiredService<IRequestHandler<SignIn, AuthDto>>();
            _passwordService = factory.Services.GetRequiredService<IPasswordService>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "identity";
        private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IRequestHandler<SignIn, AuthDto> _requestHandler;
        private readonly IPasswordService _passwordService;

        [Fact]
        public async Task signin_request_should_sign_in_user_with_correct_username_and_password()
        {
            var id = Guid.NewGuid();
            var email = "logintest@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, "test.nl/image", _passwordService.Hash(password), role,
                securityStamp, DateTime.UtcNow, new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            var request = new SignIn(email, password);

            var requestResult = _requestHandler
                .Awaiting(c => c.HandleAsync(request));

            requestResult.Should().NotThrow();

            var result = await requestResult();
            result.Should().NotBeNull();
            result.Role.Should().Be(role);
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.Expires.Should().BeGreaterThan(0);
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }


        [Fact]
        public async Task signin_request_fails_when_false_email_is_sent()
        {
            var id = Guid.NewGuid();
            var email = "test@mail";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, "test.nl/image", _passwordService.Hash(password), role,
                securityStamp, DateTime.UtcNow, new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            var request = new SignIn(email, password);

            // Check if exception is thrown
            var requestResult = _requestHandler
                .Awaiting(c => c.HandleAsync(request));

            requestResult.Should().Throw<InvalidEmailException>();
        }

        [Fact]
        public async Task signin_request_fails_when_password_is_incorrect()
        {
            var id = Guid.NewGuid();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var wrongPassword = "verysecret";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, "test.nl/image", _passwordService.Hash(password), role,
                securityStamp, DateTime.UtcNow, new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            var request = new SignIn(email, wrongPassword);

            // Check if exception is thrown
            var requestResult = _requestHandler
                .Awaiting(c => c.HandleAsync(request));

            requestResult.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public async Task signin_request_fails_when_user_does_not_exist()
        {
            var email = "test@mail.com";
            var password = "secret";


            var request = new SignIn(email, password);

            // Check if exception is thrown
            var requestResult = _requestHandler
                .Awaiting(c => c.HandleAsync(request));

            requestResult.Should().Throw<InvalidCredentialsException>();
        }
    }
}