using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Spirebyte.Services.Identity.Core.Identity;
using Spirebyte.Services.Identity.Infrastructure.EF.DbContexts;
using Spirebyte.Services.Identity.Infrastructure.EF.Entities.Identity;

namespace Spirebyte.Services.Identity.Infrastructure.EF.Queries.Handlers;

public class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<UserDto>>
{
    private readonly AdminIdentityDbContext _dbContext;
    private readonly IMessageBroker _messageBroker;

    public GetUsersHandler(AdminIdentityDbContext dbContext,
        IMessageBroker messageBroker)
    {
        _dbContext = dbContext;
        _messageBroker = messageBroker;
    }

    public async Task<IEnumerable<UserDto>> HandleAsync(GetUsers query,
        CancellationToken cancellationToken = default)
    {
        var userDtos = new List<UserDto>();

        var users = await _dbContext.Users.Where(c => string.IsNullOrEmpty(query.SearchTerm) || (c.NormalizedEmail.Contains(query.SearchTerm.ToUpperInvariant()) || c.NormalizedUserName.Contains(query.SearchTerm.ToUpperInvariant()))).ToListAsync(cancellationToken: cancellationToken);

        foreach (var user in users)
        {
            var userDto = await GetUserById(user.Id);
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