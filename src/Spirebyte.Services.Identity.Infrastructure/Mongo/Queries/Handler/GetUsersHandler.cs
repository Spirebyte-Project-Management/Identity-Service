using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<UserDto>>
{
    private readonly IMongoRepository<UserDocument, Guid> _repository;

    public GetUsersHandler(IMongoRepository<UserDocument, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserDto>> HandleAsync(GetUsers query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(query.SearchTerm))
        {
            var allUsers = (List<UserDocument>)await _repository.FindAsync(c => true);
            return allUsers?.Select(u => u.AsDto());
        }

        var filteredUsers =
            await _repository.FindAsync(u => u.NormalizedEmail.Contains(query.SearchTerm.ToUpperInvariant()));

        return filteredUsers?.Select(u => u.AsDto());
    }
}