using System;
using System.Text.Json.Serialization;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Users.Commands;

[Contract]
public class UpdateUser : ICommand
{
    public UpdateUser(Guid userId, string preferredUsername, string file)
    {
        UserId = userId;
        PreferredUsername = preferredUsername;
        File = file;
    }

    public Guid UserId { get; init; }

    [JsonPropertyName("preferred_username")]
    public string PreferredUsername { get; init; }

    public string File { get; init; }
}