using Convey.Persistence.MongoDB;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.Requests;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
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

namespace Spirebyte.Services.Identity.Tests.EndToEnd.Requests
{
    [Collection("Spirebyte collection")]
    public class SignInTests : IDisposable
    {
        private Task<HttpResponseMessage> Act(SignIn request)
            => _httpClient.PostAsync("sign-in", GetContent(request));

        public SignInTests(SpirebyteApplicationEndToEndFactory<Program> factory)
        {
            var mongoOptions = factory.Services.GetRequiredService<MongoDbOptions>();
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users", mongoOptions);
            factory.Server.AllowSynchronousIO = true;
            _httpClient = factory.CreateClient();
            _passwordService = factory.Services.GetRequiredService<IPasswordService>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private static StringContent GetContent(object value)
            => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

        private readonly HttpClient _httpClient;
        private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
        private readonly IPasswordService _passwordService;

        [Fact]
        public async Task signin_endpoint_should_return_http_status_code_ok_and_valid_auth_dto()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, "test.nl/image", _passwordService.Hash(password), role,
                securityStamp, 0, DateTime.MinValue, DateTime.UtcNow, new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            var request = new SignIn(email, password);

            var response = await Act(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().NotBeEmpty();
        }


        [Fact]
        public async Task signup_endpoint_should_return_error_when_email_is_invalid()
        {
            var id = new AggregateId();
            var email = "test@mail";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, "test.nl/image", _passwordService.Hash(password), role,
                securityStamp, 0, DateTime.MinValue, DateTime.UtcNow, new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            var request = new SignIn(email, password);

            var response = await Act(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();

            var exception = new InvalidEmailException(email);
            content.Should().Contain(exception.Code);
        }

        [Fact]
        public async Task signup_endpoint_should_return_error_when_password_is_incorrect()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var wrongPassword = "verysecret";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, "test.nl/image", _passwordService.Hash(password), role,
                securityStamp, 0, DateTime.MinValue, DateTime.UtcNow, new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            var request = new SignIn(email, wrongPassword);

            var response = await Act(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();

            var exception = new InvalidCredentialsException(email);
            content.Should().Contain(exception.Code);
        }

        [Fact]
        public async Task signup_endpoint_should_return_error_when_user_does_not_exist()
        {
            var email = "test@mail.com";
            var password = "secret";


            var request = new SignIn(email, password);

            var response = await Act(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();

            var exception = new InvalidCredentialsException(email);
            content.Should().Contain(exception.Code);
        }
    }
}