using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Spirebyte.Services.Identity.Core.Entities;

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
