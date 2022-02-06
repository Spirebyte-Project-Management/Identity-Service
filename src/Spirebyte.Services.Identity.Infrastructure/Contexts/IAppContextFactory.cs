using Spirebyte.Services.Identity.Application.Contexts;

namespace Spirebyte.Services.Identity.Infrastructure.Contexts;

public interface IAppContextFactory
{
    IAppContext Create();
}