using System;
using System.Collections.Generic;
using System.Text;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
            => builder
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher();
    }
}
