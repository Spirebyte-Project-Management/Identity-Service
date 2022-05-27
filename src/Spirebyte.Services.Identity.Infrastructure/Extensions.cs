using System;
using System.Collections.Generic;
using System.Linq;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.Redis;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Open.Serialization.Json;
using Partytitan.Convey.Minio;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Spirebyte.Services.Identity.Application;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.Commands;
using Spirebyte.Services.Identity.Application.Users.Mappers;
using Spirebyte.Services.Identity.Core.Identity;
using Spirebyte.Services.Identity.Infrastructure.Decorators;
using Spirebyte.Services.Identity.Infrastructure.EF;
using Spirebyte.Services.Identity.Infrastructure.EF.DbContexts;
using Spirebyte.Services.Identity.Infrastructure.EF.Entities.Identity;
using Spirebyte.Services.Identity.Infrastructure.Exceptions;
using Spirebyte.Services.Identity.Infrastructure.ServiceDiscovery;
using Spirebyte.Services.Identity.Infrastructure.Services;
using Spirebyte.Shared.Contexts;
using Spirebyte.Shared.IdentityServer;

namespace Spirebyte.Services.Identity.Infrastructure;

public static class Extensions
{
    public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
    {
        builder.Services.AddTransient<IMessageBroker, MessageBroker>();
        builder.Services.AddDataProtection();
        builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
        builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

        
        
        builder.Services.RegisterDbContexts(builder);
        
        builder.Services.AddSharedContexts();
        
        return builder
            .AddErrorHandler<ExceptionToResponseMapper>()
            .AddQueryHandlers()
            .AddInMemoryQueryDispatcher()
            .AddInMemoryDispatcher()
            .AddIdentityServerAuthentication()
            .AddHttpClient()
            .AddCustomConsul()
            .AddFabio()
            .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
            .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
            .AddMessageOutbox()
            .AddRedis()
            .AddMetrics()
            .AddJaeger()
            .AddWebApiSwaggerDocs()
            .AddMinio()
            .AddSecurity();
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseErrorHandler()
            .UseSwaggerDocs()
            .UseJaeger()
            .UseConvey()
            .UseMetrics()
            .UsePublicContracts<ContractAttribute>()
            .UseAuthentication()
            .UseRabbitMq()
            .SubscribeCommand<UpdateUser>();

        return app;
    }

    internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
    {
        if (accessor.HttpContext is null) return null;

        if (!accessor.HttpContext.Request.Headers.TryGetValue("x-correlation-context", out var json)) return null;

        var jsonSerializer = accessor.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();
        var value = json.FirstOrDefault();

        return string.IsNullOrWhiteSpace(value) ? null : jsonSerializer.Deserialize<CorrelationContext>(value);
    }

    public static string GetUserIpAddress(this HttpContext context)
    {
        if (context is null) return string.Empty;

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (context.Request.Headers.TryGetValue("x-forwarded-for", out var forwardedFor))
        {
            var ipAddresses = forwardedFor.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (ipAddresses.Any()) ipAddress = ipAddresses[0];
        }

        return ipAddress ?? string.Empty;
    }
}