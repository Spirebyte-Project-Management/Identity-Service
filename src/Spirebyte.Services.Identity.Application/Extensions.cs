using System;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.Types;
using Convey.WebApi.Requests;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Spirebyte.Services.Identity.Application;

public static class Extensions
{
    public static IConveyBuilder AddApplication(this IConveyBuilder builder)
    {
        return builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddRequestHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher()
            .AddInMemoryRequestDispatcher();
    }

    public static IConveyBuilder AddRequestHandlers(this IConveyBuilder builder)
    {
        builder.Services.Scan((Action<ITypeSourceSelector>)(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses((Action<IImplementationTypeFilter>)(c =>
                    c.AssignableTo(typeof(IRequestHandler<,>)).WithoutAttribute(typeof(DecoratorAttribute))))
                .AsImplementedInterfaces().WithTransientLifetime()));
        return builder;
    }

    public static IConveyBuilder AddInMemoryRequestDispatcher(
        this IConveyBuilder builder)
    {
        builder.Services.AddSingleton<IRequestDispatcher, RequestDispatcher>();
        return builder;
    }

    public static string GetMimeTypeFromBase64(string base64Url)
    {
        var pFrom = base64Url.IndexOf("data:", StringComparison.Ordinal) + "data:".Length;
        var pTo = base64Url.LastIndexOf(";", StringComparison.Ordinal);

        return base64Url.Substring(pFrom, pTo - pFrom);
    }

    public static string GetDataFromBase64(string base64Url)
    {
        var pFrom = base64Url.IndexOf("base64,", StringComparison.Ordinal) + "base64,".Length;

        return base64Url.Substring(pFrom);
    }

    public static double ConvertToUnixTimestamp(this DateTime date)
    {
        var diff = date.ToUniversalTime() - DateTime.UnixEpoch;
        return Math.Floor(diff.TotalSeconds);
    }
}