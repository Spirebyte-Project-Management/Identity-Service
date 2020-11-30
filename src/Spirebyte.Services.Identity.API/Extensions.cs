using Convey.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API.RequestDispatcher;
using Spirebyte.Services.Identity.API.RequestDispatcher.Interfaces;
using System;

namespace Spirebyte.Services.Identity.API
{
    public static class Extensions
    {
        public static IApplicationBuilder UseExtendedDispatcherEndpoints(this IApplicationBuilder app,
            Action<IExtendedDispatcherEndpointsBuilder> builder, bool useAuthorization = true,
            Action<IApplicationBuilder> middleware = null)
        {
            var definitions = app.ApplicationServices.GetService<WebApiEndpointDefinitions>();
            app.UseRouting();
            if (useAuthorization)
            {
                app.UseAuthorization();
            }

            middleware?.Invoke(app);

            app.UseEndpoints(router => builder(new ExtendedDispatcherEndpointsBuilder(
                new EndpointsBuilder(router, definitions))));

            return app;
        }
    }
}
