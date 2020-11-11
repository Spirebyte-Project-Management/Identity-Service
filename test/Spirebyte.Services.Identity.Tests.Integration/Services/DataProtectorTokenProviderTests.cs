using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Spirebyte.Services.Identity.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Integration.Services
{
    [Collection("Spirebyte collection")]
    public class DataProtectorTokenProviderTests : IDisposable
    {
        public DataProtectorTokenProviderTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _mongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _dataProtectorTokenProvider = factory.Services.GetRequiredService<IDataProtectorTokenProvider>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "identity";
        private readonly MongoDbFixture<UserDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IDataProtectorTokenProvider _dataProtectorTokenProvider;


        [Fact]
        public async Task dataprotectortokenprovider_should_be_able_to_validate_given_token()
        {
            var id = new AggregateId();
            var purpose = "purpose";
            var securityStamp = new Guid().ToString();

            var generatedToken = await _dataProtectorTokenProvider.GenerateAsync(purpose, id, securityStamp);
            var result = await _dataProtectorTokenProvider.ValidateAsync(purpose, generatedToken, id, securityStamp);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task dataprotectortokenprovider_should_generate_token_from_given_values()
        {
            var id = new AggregateId();
            var purpose = "purpose";
            var securityStamp = new Guid().ToString();

            var generatedToken = await _dataProtectorTokenProvider.GenerateAsync(purpose, id, securityStamp);

            generatedToken.Should().NotBeNullOrEmpty();
        }
    }
}