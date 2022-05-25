using System;

namespace Spirebyte.Services.Identity.Application.PersonalAccessTokens.Services.Interfaces;

public interface ITokenRequestStorage
{
    void SetToken(Guid referenceId, string token);
    string GetToken(Guid referenceId);
}