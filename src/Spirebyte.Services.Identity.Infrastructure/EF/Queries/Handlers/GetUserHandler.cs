﻿using System.Linq;
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

public class GetUserHandler : IQueryHandler<GetUser, UserDto>
{
    private readonly AdminIdentityDbContext _dbContext;
    private readonly IMessageBroker _messageBroker;

    public GetUserHandler(AdminIdentityDbContext dbContext,
        IMessageBroker messageBroker)
    {
        _dbContext = dbContext;
        _messageBroker = messageBroker;
    }

    public async Task<UserDto> HandleAsync(GetUser query, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FindAsync(query.UserId);
        var userClaims = await _dbContext.UserClaims.Where(c => c.UserId == query.UserId).ToListAsync(cancellationToken: cancellationToken);

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