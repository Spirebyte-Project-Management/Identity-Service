using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.Commands;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
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
    public class SignUpTests : IDisposable
    {
        public SignUpTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<SignUp>>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "identity";
        private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly ICommandHandler<SignUp> _commandHandler;


        [Fact]
        public async Task signup_command_fails_when_user_with_email_alreadt_exists_in_database()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, "test.nl/image", password, role, securityStamp, DateTime.UtcNow,
                new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            // Add user with same email
            var command = new SignUp(id, email, fullname, "test.nl/image", password, role, new string[] { });

            // Check if exception is thrown
            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<EmailInUseException>();
        }

        [Fact]
        public async Task signup_command_should_add_user_with_given_data_to_database()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;

            var command = new SignUp(id, email, fullname, "test.nl/image", password, role, new string[] { });

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var user = await _mongoDbFixture.GetAsync(command.UserId);

            user.Should().NotBeNull();
            user.Id.Should().Be(command.UserId);
            user.Email.Should().Be(command.Email);
            user.Fullname.Should().Be(command.Fullname);
            user.Password.Should().NotBe(command.Password);
            user.Role.Should().Be(role);
        }
    }
}