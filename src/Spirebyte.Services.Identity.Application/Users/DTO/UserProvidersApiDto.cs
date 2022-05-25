using System.Collections.Generic;

namespace Spirebyte.Services.Identity.Application.Users.DTO;

public class UserProvidersApiDto<TKey>
{
    public UserProvidersApiDto()
    {
        Providers = new List<UserProviderApiDto<TKey>>();
    }

    public List<UserProviderApiDto<TKey>> Providers { get; set; }
}