using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Identity.Application.PersonalAccessTokens.Queries.Handlers;

public class GetTokensHandler : IQueryHandler<GetTokens, IEnumerable<PersistedGrant>>
{
    private readonly IAppContext _appContext;
    private readonly IPersistedGrantStore _persistedGrantStore;

    public GetTokensHandler(IPersistedGrantStore persistedGrantStore, IAppContext appContext)
    {
        _persistedGrantStore = persistedGrantStore;
        _appContext = appContext;
    }

    public async Task<IEnumerable<PersistedGrant>> HandleAsync(GetTokens query,
        CancellationToken cancellationToken = new())
    {
        var persistedGrantFilter = new PersistedGrantFilter
        {
            SubjectId = _appContext.Identity.Id.ToString()
        };
        return await _persistedGrantStore.GetAllAsync(persistedGrantFilter);
    }
}