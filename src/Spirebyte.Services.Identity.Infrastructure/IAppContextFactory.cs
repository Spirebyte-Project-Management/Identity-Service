using Spirebyte.Services.Identity.Application;

namespace Spirebyte.Services.Identity.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}