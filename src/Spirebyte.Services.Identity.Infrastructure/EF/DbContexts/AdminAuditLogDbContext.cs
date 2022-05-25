using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;

namespace Spirebyte.Services.Identity.Infrastructure.EF.DbContexts;

public class AdminAuditLogDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
{
    public AdminAuditLogDbContext(DbContextOptions<AdminAuditLogDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    public DbSet<AuditLog> AuditLog { get; set; }
}