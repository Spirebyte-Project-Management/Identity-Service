using Spirebyte.Services.Identity.API;
using Spirebyte.Services.Identity.Tests.Shared.Factories;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]

namespace Spirebyte.Services.Identity.Tests.Integration;

[CollectionDefinition("Spirebyte collection", DisableParallelization = true)]
public class SpirebyteCollection : ICollectionFixture<SpirebyteApplicationIntegrationFactory<Program>>
{
}