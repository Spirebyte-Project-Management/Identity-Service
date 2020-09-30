using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Repositories;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Repositories
{
    internal sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoRepository<RefreshTokenDocument, Guid> _repository;

        public RefreshTokenRepository(IMongoRepository<RefreshTokenDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<RefreshToken> GetAsync(string token)
        {
            var refreshToken = await _repository.GetAsync(x => x.Token == token);

            return refreshToken?.AsEntity();
        }

        public Task AddAsync(RefreshToken token) => _repository.AddAsync(token.AsDocument());

        public Task UpdateAsync(RefreshToken token) => _repository.UpdateAsync(token.AsDocument());
    }
}
