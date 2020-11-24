using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.EndToEnd
{
    [CollectionDefinition("Spirebyte collection")]
    public class SpirebyteCollection : ICollectionFixture<SpirebyteApplicationFactory<Program>>
    {
    }
}