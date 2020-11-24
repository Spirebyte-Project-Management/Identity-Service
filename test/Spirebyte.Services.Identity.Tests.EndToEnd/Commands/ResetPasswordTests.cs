using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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

namespace Spirebyte.Services.Identity.Tests.EndToEnd.Commands
{
    [Collection("Spirebyte collection")]
    public class ResetPasswordTests : IDisposable
    {
        private Task<HttpResponseMessage> Act(ResetPassword command)
            => _httpClient.PostAsync("reset-password", GetContent(command));

        public ResetPasswordTests(SpirebyteApplicationFactory<Program> factory)
        {
            _dataProtectorTokenProvider = factory.Services.GetRequiredService<IDataProtectorTokenProvider>();
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
        private readonly string Purpose = "resetpassword";
        private readonly IDataProtectorTokenProvider _dataProtectorTokenProvider;

        [Fact]
        public async Task resetpassword_endpoint_should_return_error_when_token_is_invalid()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var newPassword = "secret";
            var pic = "test.nl/image";
            var role = Role.User;
            var securityStamp = Guid.NewGuid().ToString();
            var invalidSecurityStamp = Guid.NewGuid().ToString();

            // Add user
            var user = new User(id, email, fullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            // generate reset token
            var token = await _dataProtectorTokenProvider.GenerateAsync(Purpose, id, invalidSecurityStamp);

            var command = new ResetPassword(id, newPassword, token);


            var response = await Act(command);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task resetpassword_endpoint_should_return_error_when_user_does_not_exist()
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


            var response = await Act(command);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();

            var exception = new UserNotFoundException(id);
            content.Should().Contain(exception.Code);
        }

        [Fact]
        public async Task resetpassword_endpoint_should_return_http_status_code_ok()
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
            var user = new User(id, email, fullname, pic, password, role, securityStamp, DateTime.UtcNow,
                new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            // generate reset token
            var token = await _dataProtectorTokenProvider.GenerateAsync(Purpose, id, securityStamp);

            var command = new ResetPassword(id, newPassword, token);

            var response = await Act(command);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}