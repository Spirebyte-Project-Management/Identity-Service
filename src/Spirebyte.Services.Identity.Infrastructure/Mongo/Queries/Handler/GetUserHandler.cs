using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Application.Users.Queries;
using Spirebyte.Services.Identity.Core.Repositories;
using Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Queries.Handler;

internal sealed class GetUserHandler : IQueryHandler<GetUser, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> HandleAsync(GetUser query, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(query.UserId);

        return user?.AsDto();
    }
}