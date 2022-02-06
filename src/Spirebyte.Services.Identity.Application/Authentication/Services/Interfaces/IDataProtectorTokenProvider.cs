using System.Threading.Tasks;
using Spirebyte.Services.Identity.Core.Entities.Base;

namespace Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;

public interface IDataProtectorTokenProvider
{
    Task<string> GenerateAsync(string purpose, AggregateId userId, string securityStamp);
    Task<bool> ValidateAsync(string purpose, string token, AggregateId userId, string securityStamp);
}