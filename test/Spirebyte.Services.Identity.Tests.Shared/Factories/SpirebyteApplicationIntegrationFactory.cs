using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics.CodeAnalysis;

namespace Spirebyte.Services.Identity.Tests.Shared.Factories
{
    [ExcludeFromCodeCoverage]
    public class SpirebyteApplicationIntegrationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder().UseEnvironment("IntegrationTests");
        }
    }
}