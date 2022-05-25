using System;
using Convey;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Extensions;
using Skoruba.AuditLogging.EntityFramework.Repositories;
using Skoruba.AuditLogging.EntityFramework.Services;
using Skoruba.IdentityServer4.Admin.EntityFramework.Configuration.Configuration;
using Skoruba.IdentityServer4.Admin.EntityFramework.Configuration.MySql;
using Skoruba.IdentityServer4.Admin.EntityFramework.Configuration.PostgreSQL;
using Skoruba.IdentityServer4.Admin.EntityFramework.Configuration.SqlServer;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;
using Spirebyte.Services.Identity.Infrastructure.AuditLogging;
using Spirebyte.Services.Identity.Infrastructure.EF.DbContexts;

namespace Spirebyte.Services.Identity.Infrastructure.EF;

public static class Extensions
{
    public static IServiceCollection RegisterDbContexts(this IServiceCollection services, IConveyBuilder builder)
    {
        services
            .AddDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext,
                IdentityServerPersistedGrantDbContext, AdminLogDbContext, AdminAuditLogDbContext,
                IdentityServerDataProtectionDbContext, AuditLog>(builder);
        return services;
    }

    public static void AddDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext,
        TLogDbContext, TAuditLoggingDbContext, TDataProtectionDbContext, TAuditLog>(this IServiceCollection services,
        IConveyBuilder builder)
        where TIdentityDbContext : DbContext
        where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
        where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
        where TLogDbContext : DbContext, IAdminLogDbContext
        where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<TAuditLog>
        where TDataProtectionDbContext : DbContext, IDataProtectionKeyContext
        where TAuditLog : AuditLog
    {
        var databaseProvider = builder.GetOptions<DatabaseProviderConfiguration>(nameof(DatabaseProviderConfiguration));
        var databaseMigrations =
            builder.GetOptions<DatabaseMigrationsConfiguration>(nameof(DatabaseMigrationsConfiguration)) ??
            new DatabaseMigrationsConfiguration();
        var connectionStrings = builder.GetOptions<ConnectionStringsConfiguration>("ConnectionStrings");

        switch (databaseProvider.ProviderType)
        {
            case DatabaseProviderType.SqlServer:
                services
                    .RegisterSqlServerDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext,
                        TLogDbContext, TAuditLoggingDbContext, TDataProtectionDbContext, TAuditLog>(connectionStrings,
                        databaseMigrations);
                break;
            case DatabaseProviderType.PostgreSQL:
                services
                    .RegisterNpgSqlDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext,
                        TLogDbContext, TAuditLoggingDbContext, TDataProtectionDbContext, TAuditLog>(connectionStrings,
                        databaseMigrations);
                break;
            case DatabaseProviderType.MySql:
                services
                    .RegisterMySqlDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext,
                        TLogDbContext, TAuditLoggingDbContext, TDataProtectionDbContext, TAuditLog>(connectionStrings,
                        databaseMigrations);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType),
                    $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderType)))}.");
        }
    }

    public static IServiceCollection AddAuditEventLogging<TAuditLoggingDbContext, TAuditLog>(
        this IServiceCollection services, IConveyBuilder builder)
        where TAuditLog : AuditLog, new()
        where TAuditLoggingDbContext : IAuditLoggingDbContext<TAuditLog>
    {
        var auditLoggingConfiguration =
            builder.GetOptions<AuditLoggingConfiguration>(nameof(AuditLoggingConfiguration));
        services.AddSingleton(auditLoggingConfiguration);

        services.AddAuditLogging(options => { options.Source = auditLoggingConfiguration.Source; })
            .AddEventData<ApiAuditSubject, ApiAuditAction>()
            .AddAuditSinks<DatabaseAuditEventLoggerSink<TAuditLog>>();

        services
            .AddTransient<IAuditLoggingRepository<TAuditLog>,
                AuditLoggingRepository<TAuditLoggingDbContext, TAuditLog>>();

        return services;
    }
}