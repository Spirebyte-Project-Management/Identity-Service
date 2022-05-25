using System.Collections.Generic;
using Convey.CQRS.Queries;
using IdentityServer4.Models;

namespace Spirebyte.Services.Identity.Application.PersonalAccessTokens.Queries;

[Contract]
public class GetTokens : IQuery<IEnumerable<PersistedGrant>>
{
    
}