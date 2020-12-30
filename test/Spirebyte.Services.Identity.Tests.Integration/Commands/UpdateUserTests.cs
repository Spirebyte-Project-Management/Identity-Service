using Convey.CQRS.Commands;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.Commands;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
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
    public class UpdateUserTests : IDisposable
    {
        public UpdateUserTests(SpirebyteApplicationIntegrationFactory<Program> factory)
        {
            var rabbitmqOptions = factory.Services.GetRequiredService<RabbitMqOptions>();
            _rabbitMqFixture = new RabbitMqFixture(rabbitmqOptions);
            var mongoOptions = factory.Services.GetRequiredService<MongoDbOptions>();
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users", mongoOptions);
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<UpdateUser>>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "identity";
        private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly ICommandHandler<UpdateUser> _commandHandler;


        [Fact]
        public async Task updateuser_command_fails_when_user_does_not_exist()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var updatedFullname = "updatedfullname";
            var password = "secret";
            var pic = "test.nl/image";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user

            var command = new UpdateUser(id, updatedFullname, pic, null);

            // Check if exception is thrown
            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<UserNotFoundException>();
        }

        [Fact]
        public async Task updateuser_command_should_modify_user_with_given_data()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var updatedFullname = "updatedfullname";
            var password = "secret";
            var pic = "test.nl/image";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            var command = new UpdateUser(id, updatedFullname, pic, null);

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var updatedUser = await _mongoDbFixture.GetAsync(command.UserId);

            updatedUser.Should().NotBeNull();
            updatedUser.Id.Should().Be(command.UserId);
            updatedUser.Fullname.Should().Be(command.Fullname);
            updatedUser.Pic.Should().Be(command.Pic);
        }
    }
}