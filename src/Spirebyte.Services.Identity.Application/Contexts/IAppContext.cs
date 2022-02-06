using System;

namespace Spirebyte.Services.Identity.Application.Contexts;

public interface IAppContext
{
    Guid RequestId { get; }
    string CorrelationId { get; }
    string TraceId { get; }
    string IpAddress { get; }
    string UserAgent { get; }
    IIdentityContext Identity { get; }
}