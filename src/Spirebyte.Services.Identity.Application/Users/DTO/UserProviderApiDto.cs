﻿namespace Spirebyte.Services.Identity.Application.Users.DTO;

public class UserProviderApiDto<TKey>
{
    public TKey UserId { get; set; }

    public string UserName { get; set; }

    public string ProviderKey { get; set; }

    public string LoginProvider { get; set; }

    public string ProviderDisplayName { get; set; }
}