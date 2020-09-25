using System.Threading.Tasks;
using Convey;
using Convey.Configurations.Vault;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.Application;
using Spirebyte.Services.Identity.Application.Commands;
using Spirebyte.Services.Identity.Infrastructure;

namespace Spirebyte.Services.Identity.API
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get("ping", ctx => ctx.Response.WriteAsync("pong"))
                        .Get<GetUser>()
                        .Post<SignUp>("sign-up", afterDispatch: (cmd, ctx) => ctx.Response.Created("identity/me"))
                    ))
                .UseLogging()
                .UseVault()
                .Build()
                .RunAsync();
    }
}
