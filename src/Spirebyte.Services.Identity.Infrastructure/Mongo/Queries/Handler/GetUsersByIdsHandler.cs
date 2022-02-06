using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetUsersByIdsHandler : IQueryHandler<GetUsersByIds, IEnumerable<UserDto>>
{
    private readonly IMongoRepository<UserDocument, Guid> _repository;


    public GetUsersByIdsHandler(IMongoRepository<UserDocument, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserDto>> HandleAsync(GetUsersByIds query,
        CancellationToken cancellationToken = default)
    {
        var documents = _repository.Collection.AsQueryable();

        var users = await documents.Where(u => query.UserIds.Contains(u.Id)).ToListAsync(cancellationToken);

        return users.Select(p => p.AsDto());
    }
}