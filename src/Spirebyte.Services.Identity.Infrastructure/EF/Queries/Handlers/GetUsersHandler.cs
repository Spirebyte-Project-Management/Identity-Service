using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using IdentityModel;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Spirebyte.Services.Identity.Core.Identity;
using Spirebyte.Services.Identity.Infrastructure.EF.Entities.Identity;

namespace Spirebyte.Services.Identity.Infrastructure.EF.Queries.Handlers;

public class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<UserDto>>
{
    private readonly IIdentityService<IdentityUserDto, IdentityRoleDto, UserIdentity, UserIdentityRole, string,
        UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken
        , IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto, IdentityUserClaimsDto, IdentityUserProviderDto,
        IdentityUserProvidersDto, IdentityUserChangePasswordDto, IdentityRoleClaimsDto, IdentityUserClaimDto,
        IdentityRoleClaimDto> _identityService;

    private readonly IMessageBroker _messageBroker;

    public GetUsersHandler(
        IIdentityService<IdentityUserDto, IdentityRoleDto, UserIdentity, UserIdentityRole, string, UserIdentityUserClaim
            , UserIdentityUserRole,
            UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
            IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto,
            IdentityUserClaimsDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto,
            IdentityRoleClaimsDto, IdentityUserClaimDto, IdentityRoleClaimDto> identityService,
        IMessageBroker messageBroker)
    {
        _identityService = identityService;
        _messageBroker = messageBroker;
    }

    public async Task<IEnumerable<UserDto>> HandleAsync(GetUsers query,
        CancellationToken cancellationToken = default)
    {
        var userDtos = new List<UserDto>();

        var users = await _identityService.GetUsersAsync(query.SearchTerm, pageSize: 10000);

        foreach (var user in users.Users)
        {
            var userDto = await GetUserById(user.Id);
            userDtos.Add(userDto);
        }

        return userDtos;
    }

    private async Task<UserDto> GetUserById(string id)
    {
        var user = await _identityService.GetUserAsync(id);
        var userClaims = await _identityService.GetUserClaimsAsync(id);

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            PreferredUsername = user.UserName,
            Picture = userClaims.Claims.FirstOrDefault(c => c.ClaimType == JwtClaimTypes.Picture)?.ClaimValue
        };

        return userDto;
    }
}