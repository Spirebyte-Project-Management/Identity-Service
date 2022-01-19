using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Application.Queries;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Spirebyte.Services.Identity.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.EndToEnd.Queries;

[Collection("Spirebyte collection")]
public class GetUsersTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;

    public GetUsersTests(SpirebyteApplicationEndToEndFactory<Program> factory)
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

    private Task<HttpResponseMessage> Act(GetUsers query)
    {
        return _httpClient.GetAsync("users");
    }

    private static StringContent GetContent(object value)
    {
        return new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
    }


    [Fact]
    public async Task get_users_endpoint_should_return_http_status_code_ok_and_user_dtos()
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
        var response = await Act(query);

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var userDto = JsonConvert.DeserializeObject<UserDto[]>(content);
        userDto.Should().NotBeNull();
        userDto.First().Id.Should().Be(user.Id);
        userDto.First().Email.Should().Be(user.Email);
        userDto.First().Fullname.Should().Be(user.Fullname);
        userDto.First().Pic.Should().Be(user.Pic);
        userDto.First().Role.Should().Be(user.Role);
    }


    [Fact]
    public async Task get_users_endpoint_should_return_ok_but_no_content()
    {
        var query = new GetUsers();

        // Check if exception is thrown

        var response = await Act(query);

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var userDto = JsonConvert.DeserializeObject<UserDto[]>(content);
        userDto.Should().BeEmpty();
    }
}