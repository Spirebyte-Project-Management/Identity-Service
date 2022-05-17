using System;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Identity.Core.Entities;

namespace Spirebyte.Services.Identity.Infrastructure.Identity;

public static class Extensions
{
    public static IConveyBuilder AddIdentity(this IConveyBuilder builder)
    {
        var mongoDbSettings = builder.Configuration.GetOptions<MongoDbOptions>("mongo");
        
        builder.Services.AddIdentity<User, Role>()
            .AddMongoDbStores<User, Role, Guid>
            (
                mongoDbSettings.ConnectionString, mongoDbSettings.Database
            );

        return builder;
    }
}