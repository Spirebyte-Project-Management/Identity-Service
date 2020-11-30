using Convey.CQRS.Queries;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class GetUsersByIdsTests : IDisposable
    {
        public GetUsersByIdsTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetUsersByIds, IEnumerable<UserDto>>>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "identity";
        private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IQueryHandler<GetUsersByIds, IEnumerable<UserDto>> _queryHandler;


        [Fact]
        public async Task get_users_by_ids_query_can_filter_based_on_ids_when_a_user_exists()
        {
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var email = "test@mail.com";
            var email1 = "test1@mail.com";
            var email2 = "test2@mail.com";
            var fullname = "fullname";
            var filterFullname = "filter";
            var password = "secret";
            var pic = "test.nl/image";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add users
            var user = new User(id, email, fullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });
            var user1 = new User(id1, email1, filterFullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });
            var user2 = new User(id2, email2, filterFullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });

            await _mongoDbFixture.InsertAsync(user.AsDocument());
            await _mongoDbFixture.InsertAsync(user1.AsDocument());
            await _mongoDbFixture.InsertAsync(user2.AsDocument());

            var query = new GetUsersByIds(new Guid[] { user.Id, user2.Id });

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            var userDtos = result as UserDto[] ?? result.ToArray();

            userDtos.Should().Contain(c => c.Id == user.Id);
            userDtos.Should().Contain(c => c.Id == user2.Id);
        }

        [Fact]
        public async Task get_users_by_ids_query_should_return_empty_when_no_users_with_specified_ids_exist()
        {
            var id = Guid.NewGuid();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var email = "test@mail.com";
            var email1 = "test1@mail.com";
            var email2 = "test2@mail.com";
            var fullname = "fullname";
            var filterFullname = "filter";
            var password = "secret";
            var pic = "test.nl/image";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add users
            var user = new User(id, email, fullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });
            var user1 = new User(id1, email1, filterFullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });
            var user2 = new User(id2, email2, filterFullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });

            await _mongoDbFixture.InsertAsync(user.AsDocument());
            await _mongoDbFixture.InsertAsync(user1.AsDocument());
            await _mongoDbFixture.InsertAsync(user2.AsDocument());

            var query = new GetUsersByIds(new Guid[] { Guid.NewGuid() });

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task get_users_by_ids_query_should_return_empty_when_no_users_exist()
        {
            var id = Guid.NewGuid();

            var query = new GetUsersByIds(new Guid[] { id });

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeEmpty();
        }
    }
}
