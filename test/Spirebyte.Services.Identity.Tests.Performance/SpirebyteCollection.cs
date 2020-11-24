using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Performance
{
    [CollectionDefinition("Spirebyte collection")]
    public class SpirebyteCollection : ICollectionFixture<SpirebyteApplicationFactory<API.Program>>
    {
    }
}