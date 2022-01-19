using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Application.Queries;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Spirebyte.Services.Identity.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Integration.Queries;

[Collection("Spirebyte collection")]
public class GetUsersTests : IDisposable
{
    private const string Exchange = "identity";
    private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
    private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _queryHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;

    public GetUsersTests(SpirebyteApplicationIntegrationFactory<Program> factory)
    {
        var rabbitmqOptions = factory.Services.GetRequiredService<RabbitMqOptions>();
        _rabbitMqFixture = new RabbitMqFixture(rabbitmqOptions);
        var mongoOptions = factory.Services.GetRequiredService<MongoDbOptions>();
        _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users", mongoOptions);
        factory.Server.AllowSynchronousIO = true;
        _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetUsers, IEnumerable<UserDto>>>();
    }

    public void Dispose()
    {
        _mongoDbFixture.Dispose();
    }


    [Fact]
    public async Task get_users_query_succeeds_when_a_user_exists()
    {
        var id = Guid.NewGuid();
        var id1 = Guid.NewGuid();
        var email = "test@mail.com";
        var email1 = "test1@mail.com";
        var fullname = "fullname";
        var password = "secret";
        var pic = "test.nl/image";
        var role = Role.User;
        var securityStamp = new Guid().ToString();

        // Add users
        var user = new User(id, email, fullname, pic, password, role, securityStamp, 0, DateTime.MinValue,
            DateTime.UtcNow,
            new string[] { });
        var user1 = new User(id1, email1, fullname, pic, password, role, securityStamp, 0, DateTime.MinValue,
            DateTime.UtcNow,
            new string[] { });

        await _mongoDbFixture.InsertAsync(user.AsDocument());
        await _mongoDbFixture.InsertAsync(user1.AsDocument());


        var query = new GetUsers();

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        requestResult.Should().NotThrow();

        var result = await requestResult();

        var userDtos = result as UserDto[] ?? result.ToArray();

        userDtos.First().Should().NotBeNull();
        userDtos.First().Id.Should().Be(user.Id);
        userDtos.First().Email.Should().Be(user.Email);
        userDtos.First().Fullname.Should().Be(user.Fullname);
        userDtos.First().Pic.Should().Be(user.Pic);
        userDtos.First().Role.Should().Be(user.Role);
    }

    [Fact]
    public async Task get_users_query_can_filter_based_on_fullname_when_a_user_exists()
    {
        var id = Guid.NewGuid();
        var id1 = Guid.NewGuid();
        var email = "test@mail.com";
        var email1 = "test1@mail.com";
        var fullname = "fullname";
        var filterFullname = "filter";
        var password = "secret";
        var pic = "test.nl/image";
        var role = Role.User;
        var securityStamp = new Guid().ToString();

        // Add users
        var user = new User(id, email, fullname, pic, password, role, securityStamp, 0, DateTime.MinValue,
            DateTime.UtcNow,
            new string[] { });
        var user1 = new User(id1, email1, filterFullname, pic, password, role, securityStamp, 0, DateTime.MinValue,
            DateTime.UtcNow,
            new string[] { });

        await _mongoDbFixture.InsertAsync(user.AsDocument());
        await _mongoDbFixture.InsertAsync(user1.AsDocument());

        var query = new GetUsers(filterFullname);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        requestResult.Should().NotThrow();

        var result = await requestResult();

        var userDtos = result as UserDto[] ?? result.ToArray();

        userDtos.First().Should().NotBeNull();
        userDtos.First().Id.Should().Be(user1.Id);
        userDtos.First().Email.Should().Be(user1.Email);
        userDtos.First().Fullname.Should().Be(user1.Fullname);
        userDtos.First().Pic.Should().Be(user1.Pic);
        userDtos.First().Role.Should().Be(user1.Role);
    }

    [Fact]
    public async Task get_users_query_should_return_empty_when_no_users_exist()
    {
        var query = new GetUsers();

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        requestResult.Should().NotThrow();

        var result = await requestResult();

        result.Should().BeEmpty();
    }
}