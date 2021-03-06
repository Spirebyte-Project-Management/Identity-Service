using Spirebyte.Services.Identity.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Identity.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}
