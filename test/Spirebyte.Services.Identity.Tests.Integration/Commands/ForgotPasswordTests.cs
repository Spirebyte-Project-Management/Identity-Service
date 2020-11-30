using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.Commands;
using Spirebyte.Services.Identity.Application.Events;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Spirebyte.Services.Identity.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Integration.Commands
{
    [Collection("Spirebyte collection")]
    public class ForgotPasswordTests : IDisposable
    {
        public ForgotPasswordTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<ForgotPassword>>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "identity";
        private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly ICommandHandler<ForgotPassword> _commandHandler;


        [Fact]
        public async Task forgotpassword_command_fails_when_user_does_not_exist()
        {
            var email = "test@mail.com";

            var command = new ForgotPassword(email);

            // Check if exception is thrown
            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<InvalidEmailException>();
        }

        [Fact(Timeout = 10000)]
        public async Task forgotpassword_command_should_create_password_forgotton_event_with_token()
        {
            var id = Guid.NewGuid();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var pic = "test.nl/image";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            var command = new ForgotPassword(email);

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var tcs = _rabbitMqFixture.Subscribe<PasswordForgotten>(Exchange);

            var passwordForgotten = await tcs.Task;
            passwordForgotten.Should().NotBeNull();
            passwordForgotten.UserId.Should().Be(user.Id);
            passwordForgotten.Fullname.Should().Be(user.Fullname);
            passwordForgotten.Email.Should().Be(user.Email);
            passwordForgotten.Token.Should().NotBeNullOrEmpty();
        }
    }
}