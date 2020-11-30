using Spirebyte.Services.Identity.Core.Entities.Base;
using System.Threading.Tasks;

namespace Spirebyte.Services.Identity.Application.Services.Interfaces
{
    public interface IDataProtectorTokenProvider
    {
        Task<string> GenerateAsync(string purpose, AggregateId userId, string securityStamp);
        Task<bool> ValidateAsync(string purpose, string token, AggregateId userId, string securityStamp);
    }
}