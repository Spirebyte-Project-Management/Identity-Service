using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Application.Queries;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Spirebyte.Services.Identity.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class GetUserTests : IDisposable
    {
        public GetUserTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetUser, UserDto>>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "identity";
        private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IQueryHandler<GetUser, UserDto> _queryHandler;


        [Fact]
        public async Task get_user_query_succeeds_when_user_exists()
        {
            var id = new AggregateId();
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


            var query = new GetUser(user.Id);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
            result.Email.Should().Be(user.Email);
            result.Fullname.Should().Be(user.Fullname);
            result.Pic.Should().Be(user.Pic);
            result.Role.Should().Be(user.Role);
        }

        [Fact]
        public async Task get_user_query_should_return_null_when_user_does_not_exist()
        {
            var id = new AggregateId();

            var query = new GetUser(id);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeNull();
        }
    }
}
