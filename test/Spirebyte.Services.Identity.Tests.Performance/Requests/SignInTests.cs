using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using Spirebyte.Services.Identity.Application.Requests;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Spirebyte.Services.Identity.Tests.Shared.Fixtures;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Performance.Requests
{
    [Collection("Spirebyte collection")]
    public class SignInTests : IDisposable
    {
        private Task<HttpResponseMessage> Act(SignIn request)
            => _httpClient.PostAsync("sign-in", GetContent(request));

        public SignInTests(SpirebyteApplicationFactory<API.Program> factory)
        {
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
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

        [Fact(Skip = "Server is not fast enough")]
        public async Task signin_endpoint_runs_performantly()
        {
            const int duration = 3;
            const int expectedRps = 50;

            var id = Guid.NewGuid();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;
            var securityStamp = new Guid().ToString();

            // Add user
            var user = new User(id, email, fullname, "test.nl/image", _passwordService.Hash(password), role,
                securityStamp, DateTime.UtcNow, new string[] { });
            await _mongoDbFixture.InsertAsync(user.AsDocument());

            var step = Step.Create("sendPost", async context =>
            {
                var request = new SignIn(email, password);
                var response = await Act(request);
                return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
            });


            var scenario = ScenarioBuilder.CreateScenario("GET resources", step)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(rate: expectedRps, during: TimeSpan.FromSeconds(duration))
                });

            var result = NBomberRunner
                .RegisterScenarios(scenario)
                .Run().ScenarioStats.First();

            result.OkCount.Should().BeGreaterOrEqualTo(expectedRps * duration);
        }
    }
}