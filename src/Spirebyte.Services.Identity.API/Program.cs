using System.Collections.Generic;
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
using Spirebyte.Services.Identity.Infrastructure;
using System.Threading.Tasks;
using Spirebyte.Services.Identity.Application.Requests;

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
                    .UsePingEndpoint()
                    .UseExtendedDispatcherEndpoints(endpoints => endpoints
                        .Post<SignIn, AuthDto>("sign-in")
                    )
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetUsersByIds, IEnumerable<UserDto>>("users/withids")
                        .Get<GetUser, UserDto>("users/{userId}")
                        .Get<GetUser, UserDto>("me",
                            beforeDispatch: async (cmd, ctx) => cmd.UserId = await ctx.AuthenticateUsingJwtAsync())
                        .Post<SignUp>("sign-up", afterDispatch: (cmd, ctx) => ctx.Response.Created("identity/me"))
                        .Post<ForgotPassword>("forgot-password")
                        .Post<ResetPassword>("reset-password")
                        .Put<UpdateUser>("me",
                            beforeDispatch: async (cmd, ctx) => cmd.UserId = await ctx.AuthenticateUsingJwtAsync())
                    ))
                .UseLogging()
                .UseVault()
                .Build()
                .RunAsync();
    }
}
