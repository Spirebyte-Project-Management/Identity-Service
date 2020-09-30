﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.CQRS.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.API.RequestDispatcher;
using Spirebyte.Services.Identity.API.RequestDispatcher.Interfaces;

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
