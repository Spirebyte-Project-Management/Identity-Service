using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.Commands;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Spirebyte.Services.Identity.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.EndToEnd.Commands
{
    [Collection("Spirebyte collection")]
    public class ForgotPasswordTests : IDisposable
    {
        private Task<HttpResponseMessage> Act(ForgotPassword command)
            => _httpClient.PostAsync("forgot-password", GetContent(command));

        public ForgotPasswordTests(SpirebyteApplicationFactory<Program> factory)
        {
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
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
        public async Task forgotpassword_endpoint_should_return_error_when_user_with_email_does_not_exist()
        {
            var email = "test@mail.com";

            var command = new ForgotPassword(email);

            // Check if exception is thrown
            var response = await Act(command);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();

            var exception = new InvalidEmailException(email);
            content.Should().Contain(exception.Code);
        }

        [Fact(Timeout = 10000)]
        public async Task forgotpassword_endpoint_should_return_http_status_code_ok()
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

            var command = new ForgotPassword(email);

            var response = await Act(command);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}