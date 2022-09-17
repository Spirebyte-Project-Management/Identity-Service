using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework;
using Spirebyte.Services.Identity.Application;
using Spirebyte.Services.Identity.Infrastructure;

namespace Spirebyte.Services.Identity.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateWebHostBuilder(args)
            .Build()
            .RunAsync();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) => services
                .AddApplication()
                .AddInfrastructure(ctx.Configuration)
                .AddControllers()
            )
            .Configure(app => app
                .UseSpirebyteFramework()
                .UseApplication()
                .UseInfrastructure()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("",
                            ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppInfo>().Name));
                        endpoints.MapGet("/ping", () => "pong");
                        endpoints.MapControllers();
                    }
                ))
            .AddSpirebyteFramework();
    }
}