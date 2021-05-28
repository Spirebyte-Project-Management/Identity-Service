using Convey.Persistence.MongoDB;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.EndToEnd.Queries
{
    [Collection("Spirebyte collection")]
    public class GetUserTests : IDisposable
    {
        private Task<HttpResponseMessage> Act(GetUser query)
            => _httpClient.GetAsync($"users/{query.UserId}");

        public GetUserTests(SpirebyteApplicationEndToEndFactory<Program> factory)
        {
            var mongoOptions = factory.Services.GetRequiredService<MongoDbOptions>();
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users", mongoOptions);
            factory.Server.AllowSynchronousIO = true;
            _httpClient = factory.CreateClient();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private static StringContent GetContent(object value)
            => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

        private readonly HttpClient _httpClient;
        private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;


        [Fact]
        public async Task get_user_endpoint_should_return_http_status_code_ok_and_user_dto()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var pic = "test.nl/image";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, pic, password, role, securityStamp, 0, DateTime.MinValue, DateTime.UtcNow,
                new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());


            var query = new GetUser(user.Id);

            // Check if exception is thrown

            var response = await Act(query);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var userDto = JsonConvert.DeserializeObject<UserDto>(content);
            userDto.Should().NotBeNull();
            userDto.Id.Should().Be(user.Id);
            userDto.Email.Should().Be(user.Email);
            userDto.Fullname.Should().Be(user.Fullname);
            userDto.Pic.Should().Be(user.Pic);
            userDto.Role.Should().Be(user.Role);
        }

        [Fact]
        public async Task get_user_endpoint_should_return_not_found_when_no_user_with_id_exists()
        {
            var id = new AggregateId();

            var query = new GetUser(id);

            // Check if exception is thrown

            var response = await Act(query);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
