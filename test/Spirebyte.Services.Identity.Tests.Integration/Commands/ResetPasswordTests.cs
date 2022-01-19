using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.Commands;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Spirebyte.Services.Identity.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Integration.Commands;

[Collection("Spirebyte collection")]
public class ResetPasswordTests : IDisposable
{
    private const string Exchange = "identity";
    private readonly ICommandHandler<ResetPassword> _commandHandler;
    private readonly IDataProtectorTokenProvider _dataProtectorTokenProvider;
    private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly string Purpose = "resetpassword";

    public ResetPasswordTests(SpirebyteApplicationIntegrationFactory<Program> factory)
    {
        var rabbitmqOptions = factory.Services.GetRequiredService<RabbitMqOptions>();
        _rabbitMqFixture = new RabbitMqFixture(rabbitmqOptions);
        var mongoOptions = factory.Services.GetRequiredService<MongoDbOptions>();
        _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users", mongoOptions);

        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<ResetPassword>>();
        _dataProtectorTokenProvider = factory.Services.GetRequiredService<IDataProtectorTokenProvider>();
    }

    public void Dispose()
    {
        _mongoDbFixture.Dispose();
    }

    [Fact]
    public async Task resetpassword_command_should_fail_if_token_is_invalid()
    {
        var id = new AggregateId();
        var email = "test@mail.com";
        var fullname = "fullname";
        var updatedFullname = "updatedfullname";
        var password = "secret";
        var newPassword = "secret";
        var pic = "test.nl/image";
        var role = Role.User;
        var securityStamp = Guid.NewGuid().ToString();
        var invalidSecurityStamp = Guid.NewGuid().ToString();

        // Add user
        var user = new User(id, email, fullname, pic, password, role, securityStamp, 0, DateTime.MinValue,
            DateTime.UtcNow,
            new string[] { });
        await _mongoDbFixture.InsertAsync(user.AsDocument());

        // generate reset token
        var token = await _dataProtectorTokenProvider.GenerateAsync(Purpose, id, invalidSecurityStamp);

        var command = new ResetPassword(id, newPassword, token);

        _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().Throw<InvalidTokenException>();
    }


    [Fact]
    public async Task resetpassword_command_should_fail_if_user_does_not_exist()
    {
        var id = new AggregateId();
        var email = "test@mail.com";
        var fullname = "fullname";
        var password = "secret";
        var newPassword = "secret";
        var pic = "test.nl/image";
        var role = Role.User;
        var securityStamp = new Guid().ToString();


        // generate reset token
        var token = await _dataProtectorTokenProvider.GenerateAsync(Purpose, id, securityStamp);

        var command = new ResetPassword(id, newPassword, token);

        _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().Throw<UserNotFoundException>();
    }

    [Fact]
    public async Task resetpassword_command_should_reset_password_given_valid_data()
    {
        var id = new AggregateId();
        var email = "test@mail.com";
        var fullname = "fullname";
        var password = "secret";
        var newPassword = "secret";
        var pic = "test.nl/image";
        var role = Role.User;
        var securityStamp = new Guid().ToString();

        // Add user
        var user = new User(id, email, fullname, pic, password, role, securityStamp, 0, DateTime.MinValue,
            DateTime.UtcNow,
            new string[] { });
        await _mongoDbFixture.InsertAsync(user.AsDocument());

        // generate reset token
        var token = await _dataProtectorTokenProvider.GenerateAsync(Purpose, id, securityStamp);

        var command = new ResetPassword(id, newPassword, token);

        _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrow();


        var updatedUser = await _mongoDbFixture.GetAsync(command.UserId);

        updatedUser.Should().NotBeNull();
        updatedUser.Id.Should().Be(command.UserId);
        updatedUser.Password.Should().NotBe(password);
    }
}