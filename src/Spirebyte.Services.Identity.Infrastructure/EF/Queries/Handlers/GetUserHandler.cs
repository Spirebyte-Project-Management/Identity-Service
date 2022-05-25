using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using IdentityModel;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Spirebyte.Services.Identity.Core.Identity;
using Spirebyte.Services.Identity.Infrastructure.EF.Entities.Identity;

namespace Spirebyte.Services.Identity.Infrastructure.EF.Queries.Handlers;

public class GetUserHandler : IQueryHandler<GetUser, UserDto>
{
    private readonly IIdentityService<IdentityUserDto, IdentityRoleDto, UserIdentity, UserIdentityRole, string,
        UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken
        , IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto, IdentityUserClaimsDto, IdentityUserProviderDto,
        IdentityUserProvidersDto, IdentityUserChangePasswordDto, IdentityRoleClaimsDto, IdentityUserClaimDto,
        IdentityRoleClaimDto> _identityService;

    public GetUserHandler(
        IIdentityService<IdentityUserDto, IdentityRoleDto, UserIdentity, UserIdentityRole, string, UserIdentityUserClaim
            , UserIdentityUserRole,
            UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
            IdentityUsersDto, IdentityRolesDto, IdentityUserRolesDto,
            IdentityUserClaimsDto, IdentityUserProviderDto, IdentityUserProvidersDto, IdentityUserChangePasswordDto,
            IdentityRoleClaimsDto, IdentityUserClaimDto, IdentityRoleClaimDto> identityService)
    {
        _identityService = identityService;
    }

    public async Task<UserDto> HandleAsync(GetUser query, CancellationToken cancellationToken = default)
    {
        var user = await _identityService.GetUserAsync(query.UserId);
        var userClaims = await _identityService.GetUserClaimsAsync(query.UserId);

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