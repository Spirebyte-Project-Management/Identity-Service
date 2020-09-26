using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Spirebyte.Services.Identity.Core.Entities;

namespace Spirebyte.Services.Identity.Core.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
    }
}
