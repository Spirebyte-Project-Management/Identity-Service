using Spirebyte.Services.Identity.Core.Entities;
using System.Threading.Tasks;

namespace Spirebyte.Services.Identity.Core.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
    }
}
