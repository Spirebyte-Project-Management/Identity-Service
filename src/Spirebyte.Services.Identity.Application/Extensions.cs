using System;
using System.Collections.Generic;
using System.Text;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.Types;
using Convey.WebApi.Requests;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Spirebyte.Services.Identity.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
            => builder
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddRequestHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher()
                .AddInMemoryRequestDispatcher();

        public static IConveyBuilder AddRequestHandlers(this IConveyBuilder builder)
        {
            builder.Services.Scan((Action<ITypeSourceSelector>)(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).AddClasses((Action<IImplementationTypeFilter>)(c => c.AssignableTo(typeof(IRequestHandler<,>)).WithoutAttribute(typeof(DecoratorAttribute)))).AsImplementedInterfaces().WithTransientLifetime()));
            return builder;
        }

        public static IConveyBuilder AddInMemoryRequestDispatcher(
            this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IRequestDispatcher, RequestDispatcher>();
            return builder;
        }
    }
}
