using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Spirebyte.Services.Identity.Infrastructure.EF.DbContexts;

namespace Spirebyte.Services.Identity.Infrastructure.EF.Queries.Handlers;

public class GetUsersByIdsHandler : IQueryHandler<GetUsersByIds, IEnumerable<UserDto>>
{
    private readonly AdminIdentityDbContext _dbContext;
    private readonly IMessageBroker _messageBroker;

    public GetUsersByIdsHandler(AdminIdentityDbContext dbContext,
        IMessageBroker messageBroker)
    {
        _dbContext = dbContext;
        _messageBroker = messageBroker;
    }

    public async Task<IEnumerable<UserDto>> HandleAsync(GetUsersByIds query,
        CancellationToken cancellationToken = default)
    {
        var userDtos = new List<UserDto>();

        foreach (var userId in query.UserIds)
        {
            var userDto = await GetUserById(userId.ToString());
            userDtos.Add(userDto);
        }

        return userDtos;
    }

    private async Task<UserDto> GetUserById(string id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        var userClaims = await _dbContext.UserClaims.Where(c => c.UserId == id).ToListAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            PreferredUsername = user.UserName,
            Picture = userClaims.FirstOrDefault(c => c.ClaimType == JwtClaimTypes.Picture)?.ClaimValue
        };

        return userDto;
    }
}