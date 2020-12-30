using Convey.Persistence.MongoDB;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.EndToEnd.Commands
{
    [Collection("Spirebyte collection")]
    public class SignUpTests : IDisposable
    {
        private Task<HttpResponseMessage> Act(SignUp command)
            => _httpClient.PostAsync("sign-up", GetContent(command));

        public SignUpTests(SpirebyteApplicationEndToEndFactory<Program> factory)
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
        public async Task signup_endpoint_should_return_error_when_user_already_exists()
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


            var response = await Act(command);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();

            var exception = new EmailInUseException(email);
            content.Should().Contain(exception.Code);
        }

        [Fact]
        public async Task signup_endpoint_should_return_http_status_code_created()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;

            var command = new SignUp(id, email, fullname, "test.nl/image", password, role, new string[] { });

            var response = await Act(command);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}