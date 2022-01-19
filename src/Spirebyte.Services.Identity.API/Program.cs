using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Secrets.Vault;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.Application;
using Spirebyte.Services.Identity.Application.Commands;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Application.Queries;
using Spirebyte.Services.Identity.Application.Requests;
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

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(services => services
                .AddConvey()
                .AddWebApi()
                .AddApplication()
                .AddInfrastructure()
                .Build())
            .Configure(app => app
                .UseInfrastructure()
                .UsePingEndpoint()
                .UseExtendedDispatcherEndpoints(endpoints => endpoints
                    .Post<SignIn, AuthDto>("sign-in")
                    .Post<UseRefreshToken, AuthDto>("refresh-tokens/use")
                )
                .UseDispatcherEndpoints(endpoints => endpoints
                    .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                    .Get<GetUsersByIds, IEnumerable<UserDto>>("users/withids")
                    .Get<GetUsers, IEnumerable<UserDto>>("users")
                    .Get<GetUser, UserDto>("users/{userId}")
                    .Get<GetUser, UserDto>("profile",
                        async (cmd, ctx) => cmd.UserId = await ctx.AuthenticateUsingJwtAsync())
                    .Post<SignUp>("sign-up", afterDispatch: (cmd, ctx) => ctx.Response.Created("identity/profile"))
                    .Post<ForgotPassword>("forgot-password")
                    .Post<ResetPassword>("reset-password")
                    .Put<UpdateUser>("profile",
                        async (cmd, ctx) => cmd.UserId = await ctx.AuthenticateUsingJwtAsync())
                ))
            .UseLogging()
            .UseVault();
    }
}