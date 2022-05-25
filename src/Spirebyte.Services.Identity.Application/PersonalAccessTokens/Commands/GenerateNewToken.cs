using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.PersonalAccessTokens.Commands;

[Contract]
public class GenerateNewToken : ICommand
{
    public Guid ReferenceId = Guid.NewGuid();

    public GenerateNewToken(string name, int expiration, string[] scopes)
    {
        Name = name;
        Expiration = expiration;
        Scopes = scopes;
    }

    public string Name { get; set; }
    public int Expiration { get; set; }
    public string[] Scopes { get; set; }
}